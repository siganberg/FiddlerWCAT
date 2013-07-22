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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace FiddlerWCAT.Helper
{
    public class CSyntaxFormatter : IFormatter
    {

        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
        private string MemberName { get; set; }

        public CSyntaxFormatter()
        {
            Context = new StreamingContext(StreamingContextStates.All);
        }

        public object Deserialize(Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            var properties = ReflectionHelper.GetProperties(graph);
            var tabs = new String('\t', 1);


            var sb = new StringBuilder();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(graph);

                if (value == null) continue;

                //-- skip ignore field 

                var ignore = prop.ProperInfo.GetCustomAttribute(typeof(XmlIgnoreAttribute), true);
                if (ignore != null) continue;

                if (IsSimpleType(value))
                {
                    var defaultValue = (DefaultValueAttribute)prop.ProperInfo.GetCustomAttribute(typeof(DefaultValueAttribute), true);
                    if (defaultValue != null && defaultValue.Value.Equals(value)) continue;

                    sb.AppendLine(String.Format(tabs + @"{0,-10} = {1};", GetMemberName(prop.ProperInfo.Name),
                                 FormatValue(value)));
                }
                else if (IsEnumerable(value))
                {
                    foreach (var obj in (IEnumerable)value)
                        foreach (var line in SerializeObject(obj, GetMemberName(prop.ProperInfo.Name)))
                            sb.AppendLine(tabs + line);
                }
                else
                {
                    foreach (var line in SerializeObject(value, GetMemberName(prop.ProperInfo.Name)))
                        sb.AppendLine(tabs + line);
                }
            }

            var sw = new StreamWriter(serializationStream);

            if (sb.Length > 0)
            {
                sw.WriteLine(MemberName ?? graph.GetType().Name.ToLower());
                sw.WriteLine("{");
                sw.WriteLine(sb);
                sw.WriteLine("}");
            }

            sw.Close();
        }

        public string GetMemberName(string origName)
        {
            var name = origName.Replace("<", "").Replace(">", "").Replace("k__BackingField", "").ToLower();
            var splitNames = name.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return splitNames[splitNames.Length - 1];
        }

        public string[] SerializeObject(object obj, string memberName)
        {
            var stream = new MemoryStream();
            var formatter = new CSyntaxFormatter { MemberName = memberName };
            formatter.Serialize(stream, obj);
            var test = Encoding.Default.GetString((stream.ToArray()));
            return test.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string FormatValue(Object obj)
        {
            //-- TODO: need to escape special characters like double quote.
            var objType = obj.GetType();

            if (objType.IsEnum)
                return Enum.GetName(objType, obj);

            var value = obj.ToString()
                .Replace("\"", @"\\\");

            return objType == typeof(bool) || objType == typeof(int)
                ? value.ToLower()
                : String.Format(@"""{0}""", value);
        }

        public bool IsSimpleType(Object obj)
        {
            return IsSimpleType(obj.GetType());
        }

        public bool IsEnumerable(Object objects)
        {
            int found = (from i in objects.GetType().GetInterfaces()
                         where i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                         select i).Count();
            return (found > 0);
        }

        public bool IsSimpleType(Type type)
        {
            return type.IsValueType ||
                   type.IsPrimitive ||
                   new List<Type> { 
                       typeof(String),
                       typeof(Decimal),
                       typeof(DateTime),
                       typeof(DateTimeOffset),
                       typeof(TimeSpan),
                       typeof(Guid)
                   }.Contains(type) ||
                   Convert.GetTypeCode(type) != TypeCode.Object;
        }


        public ISurrogateSelector SurrogateSelector { get; set; }
    }
}