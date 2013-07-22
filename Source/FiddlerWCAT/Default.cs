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

namespace FiddlerWCAT
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
        
        [DefaultValue(true)]
        public bool Cookies { get; set; }

        [DefaultValue(Verb.Get)]
        public Verb Verb { get; set; }

        [DefaultValue(false)]
        public bool Secure { get; set; }
        public string Server { get; set; }

        public Default()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
            Cookies = true; 
        }
    }
}