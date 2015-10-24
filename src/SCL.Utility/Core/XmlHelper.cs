using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Utility.Core
{
    public class XmlHelper
    {
        private XmlHelper()
        {
        }

        public static XPathNavigator CreateXPathNavigator(XmlReader reader)
        {
            return new XPathDocument(reader).CreateNavigator();
        }

        public static XPathNodeIterator GetIterator(XPathNavigator navigator, string xPath)
        {
            return (XPathNodeIterator) navigator.Evaluate(xPath);
        }

        public static XmlReader GetXmlReader(FileInfo fileInfo)
        {
            return new XmlTextReader(fileInfo.FullName);
        }

        public static XmlNode GetNode(XmlNode xmlNode, string XPath)
        {
            try
            {
                return xmlNode.SelectSingleNode(XPath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static XmlNode GetNode(XmlNode xmlNode, string XPath, XmlNamespaceManager nsManager)
        {
            try
            {
                return xmlNode.SelectSingleNode(XPath, nsManager);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(XmlDocument document, string XPath, string defaultValue)
        {
            try
            {
                return GetNodeValue(document.DocumentElement, XPath, defaultValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(XmlNode element, string XPath, string defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return xmlNode.InnerText;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static int GetNodeValue(XmlNode element, string XPath, int defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return int.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static int? GetNodeValue(XmlNode element, string XPath, int? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return int.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static float GetNodeValue(XmlNode element, string XPath, float defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return float.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static float? GetNodeValue(XmlNode element, string XPath, float? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return float.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static double GetNodeValue(XmlNode element, string XPath, double defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return double.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static double? GetNodeValue(XmlNode element, string XPath, double? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return double.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool GetNodeValue(XmlNode element, string XPath, bool defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return bool.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool? GetNodeValue(XmlNode element, string XPath, bool? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return bool.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Guid GetNodeValue(XmlNode element, string XPath, Guid defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                try
                {
                    return new Guid(xmlNode.InnerText);
                }
                catch
                {
                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Guid? GetNodeValue(XmlNode element, string XPath, Guid? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                try
                {
                    return new Guid(xmlNode.InnerText);
                }
                catch
                {
                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static DateTime GetNodeValue(XmlNode element, string XPath, DateTime defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return DateTime.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static DateTime? GetNodeValue(XmlNode element, string XPath, DateTime? defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                return DateTime.Parse(xmlNode.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(XmlDocument document, string XPath, XmlNamespaceManager nsManager,
            string defaultValue)
        {
            try
            {
                return GetNodeValue(document.DocumentElement, XPath, nsManager, defaultValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(XmlNode element, string XPath, XmlNamespaceManager nsManager,
            string defaultValue)
        {
            try
            {
                var xmlNode = element.SelectSingleNode(XPath, nsManager);
                if (xmlNode == null)
                    return defaultValue;
                return xmlNode.InnerText;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue, bool useOuter)
        {
            try
            {
                var xmlNode = document.SelectSingleNode(XPath);
                if (xmlNode == null)
                    return defaultValue;
                if (useOuter)
                    return xmlNode.OuterXml;
                return xmlNode.InnerXml;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue)
        {
            return GetNodeXML(document, XPath, defaultValue, false);
        }

        public static string GetAttribute(XmlNode node, string attributeName)
        {
            return GetAttribute(node, attributeName, "");
        }

        public static string GetAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return xmlAttribute.Value;
        }

        public static Guid GetAttribute(XmlNode node, string attributeName, Guid defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return new Guid(xmlAttribute.Value);
        }

        public static double GetAttribute(XmlNode node, string attributeName, double defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return double.Parse(xmlAttribute.Value);
        }

        public static int GetAttribute(XmlNode node, string attributeName, int defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return int.Parse(xmlAttribute.Value);
        }

        public static long GetAttribute(XmlNode node, string attributeName, long defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return long.Parse(xmlAttribute.Value);
        }

        public static bool GetAttribute(XmlNode node, string attributeName, bool defaultValue)
        {
            var xmlAttribute = node.Attributes[attributeName] ?? node.Attributes[attributeName.ToLower()];
            if (xmlAttribute == null)
                return defaultValue;
            return bool.Parse(xmlAttribute.Value);
        }

        public static XmlNode AddElement(XmlElement element, string name, string value)
        {
            var element1 = element.OwnerDocument.CreateElement(name);
            if (value.GetType() == typeof (string))
            {
                if (!string.IsNullOrEmpty(value))
                    element1.InnerText = value;
            }
            else
                element1.InnerText = value;
            return element.AppendChild(element1);
        }

        public static XmlNode AddElement(XmlDocument document, string name, string value)
        {
            var element = document.CreateElement(name);
            if (value.GetType() == typeof (string))
            {
                if (!string.IsNullOrEmpty(value))
                    element.InnerText = value;
            }
            else
                element.Value = value;
            return document.AppendChild(element);
        }

        public static XmlNode AddElement(XmlNode element, string name, string value)
        {
            return AddElement((XmlElement) element, name, value);
        }

        public static XmlNode AddElement(XmlElement element, string name)
        {
            return AddElement((XmlNode) element, name);
        }

        public static XmlNode AddElement(XmlNode element, string name)
        {
            var element1 = element.OwnerDocument.CreateElement(name);
            return element.AppendChild(element1);
        }

        public static XmlNode AddElement(XmlDocument XMLDocument, string name)
        {
            var element = XMLDocument.CreateElement(name);
            return XMLDocument.AppendChild(element);
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.InnerText = value;
            node.Attributes.Append(attribute);
            return attribute;
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, bool value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, double value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, Guid value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, int value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, bool value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, double value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, Guid value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, int value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, string value)
        {
            return AddAttribute((XmlNode) node, name, value);
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, long value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static void RemoveElement(XmlDocument document, string XPath)
        {
            foreach (XmlElement xmlElement in document.SelectNodes(XPath))
            {
                var parentNode = xmlElement.ParentNode;
                xmlElement.RemoveAll();
                parentNode.RemoveChild(xmlElement);
            }
        }

        public static void RemoveElement(XmlElement element)
        {
            element.ParentNode.RemoveChild(element);
        }

        public static void RemoveAttribute(XmlElement element, string attributeName)
        {
            var node = (XmlAttribute) element.Attributes.GetNamedItem(attributeName);
            element.Attributes.Remove(node);
        }

        public static void UpdateElement(XmlElement element, string newValue)
        {
            element.InnerText = newValue;
        }

        public static void UpdateElement(ref XmlDocument XMLDocument, string Xpath, string newValue)
        {
            XMLDocument.SelectSingleNode(Xpath).InnerText = newValue;
        }

        public static void UpdateAttribute(XmlElement XmlElement, string attributeName, string newValue)
        {
            XmlElement.Attributes.GetNamedItem(attributeName).InnerText = newValue;
        }

        public static XmlElement GetElement(XmlElement parentElement, string tagName)
        {
            var elementsByTagName = parentElement.GetElementsByTagName(tagName);
            if (elementsByTagName.Count > 0)
                return (XmlElement) elementsByTagName[0];
            return null;
        }

        public static string GetChildValue(XmlNode parentNode, string childName)
        {
            return GetChildValue(parentNode, childName, null);
        }

        public static double GetChildValue(XmlNode parentNode, string childName, double defaultValue)
        {
            var xmlNode = parentNode.SelectSingleNode(childName);
            if (xmlNode != null)
                return double.Parse(xmlNode.InnerText);
            return defaultValue;
        }

        public static int GetChildValue(XmlNode parentNode, string childName, int defaultValue)
        {
            var xmlNode = parentNode.SelectSingleNode(childName);
            if (xmlNode != null)
                return int.Parse(xmlNode.InnerText);
            return defaultValue;
        }

        public static bool GetChildValue(XmlNode parentNode, string childName, bool defaultValue)
        {
            var xmlNode = parentNode.SelectSingleNode(childName);
            if (xmlNode != null)
                return bool.Parse(xmlNode.InnerText);
            return defaultValue;
        }

        public static Guid GetChildValue(XmlNode parentNode, string childName, Guid defaultValue)
        {
            var xmlNode = parentNode.SelectSingleNode(childName);
            if (xmlNode != null)
                return new Guid(xmlNode.InnerText);
            return defaultValue;
        }

        public static string GetChildValue(XmlNode parentNode, string childName, string defaultValue)
        {
            var xmlNode = parentNode.SelectSingleNode(childName);
            if (xmlNode != null)
                return xmlNode.InnerText;
            return defaultValue;
        }
    }
}