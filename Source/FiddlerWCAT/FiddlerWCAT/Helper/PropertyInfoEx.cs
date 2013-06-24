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
using System.Reflection;

namespace FiddlerWCAT.Helper
{
    public class PropertyInfoEx
    {
        public PropertyInfo ProperInfo { get; set; }
        public Func<object, object> GetValue { get; set; }
        public Action<Object, Object> SetValue { get; set; }
    }
}