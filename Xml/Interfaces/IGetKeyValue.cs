using System.Xml.Linq;

namespace Xml.Interfaces
{
    internal interface IGetKeyValue
    {
        List<KeyValuePair<string, string>> GetKeyValue(XDocument xml);
    }
}
