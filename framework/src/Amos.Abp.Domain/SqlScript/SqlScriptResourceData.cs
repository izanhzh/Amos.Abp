using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amos.Abp.SqlScript
{
    [XmlRoot("SqlScripts")]
    public class SqlScriptResourceData
    {
        [XmlRoot("Item")]
        public class Item
        {
            [XmlAttribute("Key")]
            public string Key { get; set; }

            [XmlText]
            public string Value { get; set; }
        }

        [XmlAttribute("Namespace")]
        public string Namespace { get; set; }

        [XmlAttribute("DatabaseProvider")]
        public string DatabaseProvider { get; set; }

        [XmlElement("Item")]
        public List<Item> Items { get; set; }
    }
}
