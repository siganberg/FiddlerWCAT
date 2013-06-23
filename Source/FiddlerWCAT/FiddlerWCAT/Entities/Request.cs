using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Request : Default
    {
        public string Url { get; set; }
        public string Id { get; set; }

        [DefaultValue("GET")]
        public string Verb { get; set; }
        public string RedirVerb { get; set; }

        [DefaultValue(false)]
        public bool Redirect { get; set; }
        
        [DefaultValue(true)]
        public bool Cookies { get; set; }

        [DefaultValue(false)]
        public bool Secure { get; set; }
        public string Server { get; set; }
        
        public Request()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
        }
    }


}