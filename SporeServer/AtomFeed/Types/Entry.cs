using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Types
{
    public class Entry
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        private DateTime _updated { get; set; }

        [XmlElement("updated")]
        public DateTime Updated 
        {
            get { return XmlConvert.ToDateTime(XmlConvert.ToString(_updated, XmlDateTimeSerializationMode.Utc), XmlDateTimeSerializationMode.Utc);  }
            set { _updated = value; }  
        }

        [XmlElement("author")]
        public Author Author { get; set; }

        [XmlElement("subtitle")]
        public string Subtitle { get; set; }

        [XmlElement("subcount")]
        public int SubCount { get; set; }

        [XmlElement("link")]
        public Link Link { get; set; }
    }
}
