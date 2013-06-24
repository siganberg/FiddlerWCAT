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

        public static void UnknownAttributeHandler(object o, XmlAttributeEventArgs e)
        {
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