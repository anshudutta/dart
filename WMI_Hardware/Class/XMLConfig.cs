using System.Collections.Generic;
using System.Xml;

namespace baileysoft.Wmi
{
    class XMLConfig
    {
        public static IEnumerable<string> GetSettings(string wmiClassName)
        {
            string xmlFilePath = System.IO.Directory.GetCurrentDirectory() + "\\settings.xml";
            var alPropertyNames = new List<string>();
            var xmldoc = new XmlDocument();
            xmldoc.Load(xmlFilePath);
            var properties = xmldoc.SelectSingleNode("//" + wmiClassName);

            for (int i = 0; i < properties.ChildNodes.Count; i++)
                alPropertyNames.Add(properties.ChildNodes[i].InnerText);
            return alPropertyNames;
        }
    }
}
