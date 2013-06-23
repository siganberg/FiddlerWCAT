using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiddlerWCAT.Entities;
using Newtonsoft.Json;

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
            var s = Encoding.Default.GetString((stream.ToArray()));

            Assert.IsTrue(true);

        }
    }
}
