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