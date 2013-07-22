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
}
