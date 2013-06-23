using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FiddlerWCAT.Helper
{
    /// <summary>
    /// Provide faster access for unknown type object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class EntityInfoFactory<T> where T : class
    {
        public static List<PropertyInfoEx> Properties { get; private set; }
        
        static EntityInfoFactory()
        {
            Properties = new List<PropertyInfoEx>();
            var type = typeof (T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var p in properties)
            {
                var prop = new PropertyInfoEx { ProperInfo = p, GetValue = BuildGetAccessor(p.GetGetMethod()), SetValue = BuildSetAccessor(p.GetSetMethod())};
                Properties.Add(prop);
            }
        }


        static Func<object, object> BuildGetAccessor(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");
            if (method == null || method.DeclaringType == null)
                throw new NullReferenceException("MethodInfo is null");
            var expr =
                Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(obj, method.DeclaringType),
                            method),
                        typeof(object)),
                    obj);

            return expr.Compile();
        }

        static Action<object, object> BuildSetAccessor(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");
            var value = Expression.Parameter(typeof(object));
                var expr =
                Expression.Lambda<Action<object, object>>(
                    Expression.Call(
// ReSharper disable AssignNullToNotNullAttribute, method is will be always not null
                        Expression.Convert(obj, method.DeclaringType),
// ReSharper restore AssignNullToNotNullAttribute
                        method,
// ReSharper disable PossiblyMistakenUseOfParamsMethod, this will always be ParameterType
                        Expression.Convert(value, method.GetParameters()[0].ParameterType)),
// ReSharper restore PossiblyMistakenUseOfParamsMethod
                    obj,
                    value);

            return expr.Compile();
        }
    }
 

    public class ReflectionHelper
    {
        public static List<PropertyInfoEx> GetProperties(Object obj)
        {
            var entityInfo = typeof(EntityInfoFactory<>).MakeGenericType(obj.GetType());
            return (List<PropertyInfoEx>)entityInfo.InvokeMember("Properties", BindingFlags.GetProperty, null, null, null);
        }
    }


    public class PropertyInfoEx
    {
        public PropertyInfo ProperInfo { get; set; }
        public Func<Object, Object> GetValue { get; set; }
        public Action<Object, Object> SetValue { get; set; }
    }
}
