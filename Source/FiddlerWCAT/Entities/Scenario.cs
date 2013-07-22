// <copyright file=" " company="Digitrish">
// Copyright (c) 2013 All Right Reserved, http://www.digitrish.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//
// <author>Francis Marasigan</author>
// <email>francis.marasigan@live.com</email>
// <date>2013-06-23</date>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using FiddlerWCAT.Helper;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Scenario
    {
        private const string Folder = "ubrs";

        public string Name { get; set; }
        public int Warmup { get; set; }
        public int Duration { get; set; }
        public int Cooldown { get; set; }

        [DefaultValue(0)]
        public int? ThrottleRps { get; set; }
        public Default Default { get; set; }
        public List<Transaction> Transaction { get; set; }

        [XmlIgnore]
        public string FilePath { get; set; }


        public Scenario()
        {
            //-- Initialize with  default Settings
            Transaction = new List<Transaction>();
            Duration = 60;
            Warmup = 10;
            Cooldown = 10;
            Default = new Default();
        }

        /// <summary>
        /// Optimize Scenario by moving common request properties and headers to default. 
        /// This will possibly produce much compact .ubr file for wcat input.
        /// </summary>
        public void Optimize()
        {
            var requests = Transaction.SelectMany(a => a.Request).ToList();
            MoveRequestPropertyToDefault(requests, a => a.Server);
            MoveRequestPropertyToDefault(requests, a => a.Port);
            MoveRequestPropertyToDefault(requests, a => a.Redirect);
            MoveRequestPropertyToDefault(requests, a => a.Secure);
            MoveRequestHeaderToDefault(requests);
        }

        /// <summary>
        /// Generic method for moving common request property value to default property value
        /// </summary>
        /// <typeparam name="TP">Property Type</typeparam>
        /// <param name="requests">List or request</param>
        /// <param name="property">Property selector  to move</param>
        private void MoveRequestPropertyToDefault<TP>(List<Request> requests, Expression<Func<Request, TP>> property)
        {
            var member = (MemberExpression)property.Body;
            var memberName = member.Member.Name;

            var defaultProperty = EntityInfoFactory<Default>.Properties.FirstOrDefault(a => a.ProperInfo.Name == memberName);
            var requestProperty = EntityInfoFactory<Request>.Properties.FirstOrDefault(a => a.ProperInfo.Name == memberName);

            if (defaultProperty == null || requestProperty == null) return;

            var servers = requests.Where(a => defaultProperty.GetValue(a) != null).Select(a => requestProperty.GetValue(a)).GroupBy(a => a).ToList();
            var highOccurance = servers.OrderByDescending(a => a.Count()).FirstOrDefault();

            if (highOccurance == null) return;

            var matchRequest = requests.Where(a => requestProperty.GetValue(a) == highOccurance.Key);
            matchRequest.ToList().ForEach(a => requestProperty.SetValue(a, null));
            defaultProperty.SetValue(Default, highOccurance.Key);
        }

        private void MoveRequestHeaderToDefault(IEnumerable<Request> requests)
        {
            var headers = requests.SelectMany(a => a.SetHeader, (a, b) => new { Request = a, Header = b }).ToList();
            var headerNameOccureMoreThanOnce = headers.GroupBy(a => a.Header.Name).Where(a => a.Count() > 1).ToList();

            foreach (var h in headerNameOccureMoreThanOnce)
            {
                var highOccuranceHeaderValue =
                    headers.Where(a => a.Header.Name == h.Key)
                           .GroupBy(a => a.Header.Value)
                           .Where(a => a.Count() > 1)
                           .OrderByDescending(a => a.Count())
                           .FirstOrDefault();


                if (highOccuranceHeaderValue != null)
                {
                    //-- copy header to default 
                    var header = new Header { Name = h.Key, Value = highOccuranceHeaderValue.Key };
                    Default.SetHeader.Add(header);

                    //-- remove the original header 
                    var matchHeader = headers.Where(a => a.Header.Name == h.Key && a.Header.Value == highOccuranceHeaderValue.Key).ToList();
                    matchHeader.ForEach(a => a.Request.SetHeader.Remove(a.Header));
                }

            }
        }

        public void Save()
        {
            var path = String.Format(@"{0}\{1}\", Settings.Instance.WcatHomeDirectory, Folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            FilePath = path + String.Format("scenario_{0}.ubr", Name);

            var fs = new FileStream(FilePath, FileMode.Create);
            var formatter = new CFormatter();
            formatter.Serialize(fs, this);
            fs.Close();
        }

        public string GetRunSyntax()
        {
            return String.Format(@"wcat.wsf -terminate -run -clients localhost -t {0}\{1} -s {2} -v {3}"
                , Folder
                , Path.GetFileName(FilePath)
                , Default.Server
                , Settings.Instance.VirtualClient);
        }


        public int Run()
        {
            //-- wcat.wsf -terminate -run -clients localhost,dmgdevv12 -t invalid_header.ubr -s eagl.spe.sony.com -v %1 
            var command = String.Format(@"-terminate -run -clients {3} -t ubrs\{0} -s {1} -v {2}", Path.GetFileName(FilePath)
                , Default.Server
                , Settings.Instance.VirtualClient
                , Settings.Instance.Clients);

            var processInfo = new ProcessStartInfo
            {
                WorkingDirectory = Settings.Instance.WcatHomeDirectory,
                FileName = "wcat.wsf",
                Arguments = command,
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();
            return  process.ExitCode;
        }
    }
}