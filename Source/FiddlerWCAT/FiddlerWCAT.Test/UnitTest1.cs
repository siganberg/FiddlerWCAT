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

using System.Collections.Generic;
using System.IO;
using System.Text;
using FiddlerWCAT.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiddlerWCAT.Entities;

namespace FiddlerWCAT.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SerializerScenario()
        {
            var scenario = new Scenario();
            scenario.Name = "test";
            scenario.ThrottleRps = 1;
            scenario.Warmup = 10;
            scenario.Cooldown = 20;
            scenario.Default = new Default { StatusCode = 200, SetHeader = new List<Header> { new Header { Name = "host", Value = "localhost"}}};

            var formatter = new CFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, scenario);
            var test = Encoding.Default.GetString((stream.ToArray()));

            Assert.IsNotNull(test);

        }
    }
}
