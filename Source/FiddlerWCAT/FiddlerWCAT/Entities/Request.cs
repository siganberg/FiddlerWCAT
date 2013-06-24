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

      
        public string RedirVerb { get; set; }

        [DefaultValue(false)]
        public bool Redirect { get; set; }
        
        public Request()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
        }
    }


}