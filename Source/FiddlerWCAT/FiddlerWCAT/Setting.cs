using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiddlerWCAT
{
    public class Setting
    {
        public string WCATHomeDirectory { get; set; }
        public int Duration { get; set; }
        public int CooldownTime { get; set; }
        public int WarmupTime { get; set; }
    }

    public class Scenario
    {
        public Default Default { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Warmup { get; set; }
        public int Duration { get; set; }
        public int Cooldown { get; set; }
        public int Throttle { get; set; }
    }


    public class Transaction
    {
        public List<Request> Requests { get; set; }
        public int Weight { get; set; }
        public string Id { get; set; }
    }

    public class Default 
    {
        public List<Header> AddHeader  { get; set; }
        public List<Header> SetHeader { get; set; }
        public string StatusCode { get; set; }
    }



    public class Request : Default
    {
        public string Url { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


}
