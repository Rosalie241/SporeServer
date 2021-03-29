using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Types
{
    public class Author
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("uri")]
        public string Uri { get; set; }
    }
}
