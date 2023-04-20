using System.Xml.Linq;
using Xml.Interfaces;

namespace Xml.Utils
{
    internal class XmlUtils : IGetDataType, IGetKeyValue, IPrintXmlKeyValues
    {
        public string GetDataType(string key, string value)
        {
            if (key.EndsWith("[]"))
            {
                return "(array)";
            }
            if (int.TryParse(value, out _))
            {
                return "(intero)";
            }
            else if (bool.TryParse(value, out _))
            {
                return "(booleano)";
            }
            else
            {
                return "(stringa)";
            }
        }

        public List<KeyValuePair<string, string>> GetKeyValue(XDocument xml)
        {
            var results = new List<KeyValuePair<string, string>>();
            int groupElementCount = 0;
            string groupName = "";

            foreach (var element in xml.Descendants())
            {
                var name = element.Name.LocalName;

                if (name == "Import")
                {
                    var nestedXmlName = element.FirstAttribute.Value;
                    var nestedXml = XDocument.Load("../../../../xml_per_prova/" + nestedXmlName);
                    results.AddRange(GetKeyValue(nestedXml));
                }
                if (name == "Group")
                {
                    groupElementCount = element.Descendants().Count();
                    groupName += element.FirstAttribute.Value + "/";
                }
                if (name == "Param")
                {
                    string paramName = element.FirstAttribute.Value;
                    string paramValue;
                    if (paramName == "longtext")
                    {
                        paramValue = element.Value;
                    }
                    else
                    {
                        paramValue = element.FirstAttribute.NextAttribute.Value;
                    }

                    string fullKey = groupName + paramName;
                    results.Add(new KeyValuePair<string, string>(fullKey, paramValue));
                }
                if (groupElementCount == 0)
                    groupName = "";
                groupElementCount--;
            }

            return results;
        }

        public void PrintXmlKeyValues(List<KeyValuePair<string, string>> results)
        {
            List<string> arrayResultStringList = new();

            var resultsArrayGroup = results
                .Where(r => r.Key.EndsWith("[]"))
                .GroupBy(r => r.Key)
                .ToList();

            foreach (var group in resultsArrayGroup)
            {
                var key = group.Key;
                var values = group.Select(r => r.Value).ToList();
                arrayResultStringList.Add("[" + string.Join(", ", values) + "]");
            }

            var distinctResults = results
                .GroupBy(r => r.Key)
                .Select(g => g.Last())
                .ToList();

            foreach (var distinctResult in distinctResults)
            {
                var value = distinctResult.Value;
                if (distinctResult.Key.EndsWith("[]"))
                {
                    distinctResult.Key.Substring(distinctResult.Key.Length - 2);
                    value = arrayResultStringList[0];
                    arrayResultStringList.Remove(arrayResultStringList[0]);
                }
                Console.WriteLine(distinctResult.Key + " = " + value + " " + GetDataType(distinctResult.Key, distinctResult.Value));
            }
        }
    }
}
