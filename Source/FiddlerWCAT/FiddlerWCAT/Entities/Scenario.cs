using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FiddlerWCAT.Helper;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Scenario 
    {
        public string Name { get; set; }
        public int Warmup { get; set; }
        public int Duration { get; set; }
        public int Cooldown { get; set; }
        public int? ThrottleRps { get; set; }
        public Default Default { get; set; }
        public List<Transaction> Transaction { get; set; }
        
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

            var defaultProperty  = EntityInfoFactory<Default>.Properties.FirstOrDefault(a => a.ProperInfo.Name == memberName);
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
            var headers = requests.SelectMany(a => a.SetHeader, (a, b) => new { Request = a, Header = b}).ToList();
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
                    var header = new Header {Name = h.Key, Value = highOccuranceHeaderValue.Key};
                    Default.SetHeader.Add(header);

                    //-- remove the original header 
                    var matchHeader = headers.Where(a => a.Header.Name == h.Key && a.Header.Value == highOccuranceHeaderValue.Key).ToList();
                    matchHeader.ForEach(a => a.Request.SetHeader.Remove(a.Header));
                }

            }
        }

        public void Save()
        {
            const string fileName = "wcat_scenario.ubr";

            var path = Settings.Instance.WcatHomeDirectory + @"\wcat_ubr\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = path + fileName;

            var fs = new FileStream(path, FileMode.Create);
            var formatter = new CFormatter();
            formatter.Serialize(fs, this);
            fs.Close();


            //-- wcat.wsf -terminate -run -clients localhost,dmgdevv12 -t invalid_header.ubr -s eagl.spe.sony.com -v %1 
            var command = String.Format(@"/c wcat.wsf -terminate -run -clients localhost -t {1} -s {2} -v {3}" 
                , Settings.Instance.WcatHomeDirectory
                , @"wcat_ubr\" + Path.GetFileName(path)
                , Default.Server    
                , Settings.Instance.VirtualClient);

            var processInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Settings.Instance.WcatHomeDirectory,
                    FileName = "cmd.exe",
                    Arguments = command
                };
            Process.Start(processInfo);

        }
    }
}