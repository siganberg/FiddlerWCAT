using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Default 
    {
        public List<Header> AddHeader  { get; set; }
        public List<Header> SetHeader { get; set; }

        [DefaultValue("HTTP10")]
        public string Version { get; set; }

        [DefaultValue(200)]
        public int? StatusCode { get; set; }

        [DefaultValue(80)]
        public int? Port { get; set; }

        public Default()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
        }
    }
}