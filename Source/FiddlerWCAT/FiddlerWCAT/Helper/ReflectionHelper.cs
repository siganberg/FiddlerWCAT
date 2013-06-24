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