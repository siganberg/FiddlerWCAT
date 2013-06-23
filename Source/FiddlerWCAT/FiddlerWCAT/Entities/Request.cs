using System;
using System.Collections.Generic;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Request : Default
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Verb { get; set; }
        public string RedirVerb { get; set; }
        public bool Redirect { get; set; }
        public bool Cookies { get; set; }
        public bool Secure { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }

        public Request()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
        }
    }


}