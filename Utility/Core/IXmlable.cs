using System.Xml;

namespace Utility.Core
{
    public interface IXmlable
    {
        void ToXml(XmlNode parentNode);

        void FromXml(XmlNode parentNode);
    }
}