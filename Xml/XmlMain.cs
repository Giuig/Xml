using System.Xml.Linq;
using Xml.Utils;

namespace Xml
{
    internal class XmlMain
    {
        static void Main()
        {
            var xmlUtils = new XmlUtils();

            var documentPath = "../../../../xml_per_prova/myConfig.xml";

            var xml = XDocument.Load(documentPath);

            var results = xmlUtils.GetKeyValue(xml);

            xmlUtils.PrintXmlKeyValues(results);
        }
    }
}
