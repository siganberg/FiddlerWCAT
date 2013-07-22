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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FiddlerWCAT.Helper
{
    public static class Serializer
    {
        #region Static Methods 

        public static T DeserializeObject<T>(string data)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var stream = new StringReader(data))
            {
                try
                {
                    return (T) serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to create object from xml string.", ex);
                }
            }
        }
        
        public static string SerializeObject<T>(T obj)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (T));
                var mSteam = new MemoryStream();
                var writer = new XmlTextWriter(mSteam, null) {Formatting = Formatting.Indented};
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                serializer.Serialize(writer, obj, ns);
                return Utf8ByteArrayToString(mSteam.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string Utf8ByteArrayToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        #endregion
    }
}