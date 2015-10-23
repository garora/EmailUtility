using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Utility.Logging;

namespace Utility.Core
{
    public class SerializeHelper
    {
        public static void XMLSerialize(string fileName, object o)
        {
            var xmlSerializer = new XmlSerializer(o.GetType());
            using (TextWriter textWriter = new StreamWriter(fileName))
                textWriter.Write(XMLSerialize(o));
        }

        public static object XMLDeserialize(string fileName, Type type)
        {
            using (TextReader textReader = new StreamReader(fileName))
                return XMLDeserializeFromString(textReader.ReadToEnd(), type);
        }

        public static string XMLSerialize(object o)
        {
            if (o.GetType() == typeof (Hashtable))
                return XMLSerializeHashtable((Hashtable) o);
            var sb = new StringBuilder();
            using (var stringWriter = new StringWriter(sb))
                new XmlSerializer(o.GetType()).Serialize(stringWriter, o);
            return sb.ToString();
        }

        public static object XMLDeserializeFromString(string xml, Type type)
        {
            if (type == typeof (Hashtable))
                return XMLDeserializeHashtable(xml);
            using (var stringReader = new StringReader(xml))
                return new XmlSerializer(type).Deserialize(stringReader);
        }

        private static Hashtable XMLDeserializeHashtable(string xml)
        {
            var hashtable = new Hashtable();
            var xmlTextReader = new XmlTextReader(new StringReader(xml));
            try
            {
                xmlTextReader.ReadStartElement("HashtableAnyType");
                while (xmlTextReader.Read())
                {
                    var xml1 = xmlTextReader.ReadElementString("Key");
                    var typeName1 =
                        (string) XMLDeserializeFromString(xmlTextReader.ReadElementString("KeyType"), typeof (string));
                    var xml2 = xmlTextReader.ReadElementString("Value");
                    var typeName2 =
                        (string) XMLDeserializeFromString(xmlTextReader.ReadElementString("ValueType"), typeof (string));
                    hashtable.Add(XMLDeserializeFromString(xml1, Type.GetType(typeName1)),
                        XMLDeserializeFromString(xml2, Type.GetType(typeName2)));
                    xmlTextReader.ReadEndElement();
                }
            }
            finally
            {
                xmlTextReader.Close();
            }
            return hashtable;
        }

        private static string XMLSerializeHashtable(Hashtable ht)
        {
            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);
            try
            {
                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("HashtableAnyType");
                foreach (var o in ht.Keys)
                {
                    xmlTextWriter.WriteStartElement("KeyValuePair");
                    xmlTextWriter.WriteElementString("Key", XMLSerialize(o));
                    xmlTextWriter.WriteElementString("KeyType", XMLSerialize(o.GetType().FullName));
                    xmlTextWriter.WriteElementString("Value", XMLSerialize(ht[o]));
                    xmlTextWriter.WriteElementString("ValueType", XMLSerialize(ht[o].GetType().FullName));
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndDocument();
            }
            finally
            {
                xmlTextWriter.Close();
            }
            return stringWriter.ToString();
        }

        public static void SoapSerialize(string fileName, object o)
        {
            using (TextWriter textWriter = new StreamWriter(fileName))
                textWriter.Write(SoapSerialize(o));
        }

        public static object SoapDeserialize(string fileName)
        {
            using (TextReader textReader = new StreamReader(fileName))
                return SoapDeserializeFromString(textReader.ReadToEnd());
        }

        public static string SoapSerialize(object o)
        {
            var str = string.Empty;
            var stringBuilder = new StringBuilder();
            using (var memStream = new MemoryStream())
            {
                new SoapFormatter().Serialize((Stream) memStream, o);
                str = StringHelper.MemoryStreamToString(memStream);
            }
            return str;
        }

        public static object SoapDeserializeFromString(string soap)
        {
            try
            {
                using (MemoryStream memoryStream = StringHelper.StringToMemoryStream(soap))
                    return new SoapFormatter().Deserialize((Stream) memoryStream);
            }
            catch (Exception ex)
            {
                MessageLog.LogError(LogClass.CommonUtil, ex);
                throw;
            }
        }
    }
}