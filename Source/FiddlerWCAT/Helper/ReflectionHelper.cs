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
using System.Reflection;

namespace FiddlerWCAT.Helper
{
    public class ReflectionHelper
    {
        public static List<PropertyInfoEx> GetProperties(Object obj)
        {
            var entityInfo = typeof(EntityInfoFactory<>).MakeGenericType(obj.GetType());
            return (List<PropertyInfoEx>)entityInfo.InvokeMember("Properties", BindingFlags.GetProperty, null, null, null);
        }
    }
}