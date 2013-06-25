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

namespace FiddlerWCAT.Entities
{
    [Serializable]
    public class Request : Default
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string PostData { get; set; }
        public string RedirVerb { get; set; }

        [DefaultValue(false)]
        public bool Redirect { get; set; }
        
        public Request()
        {
            AddHeader = new List<Header>();
            SetHeader = new List<Header>();
        }

     
    }

    public enum Verb
    {
        GET,
        POST
    }
}