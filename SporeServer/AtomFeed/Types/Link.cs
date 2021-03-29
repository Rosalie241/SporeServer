using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Types
{
    public class Link
    {
        public Link()
        {

        }

        public Link(string url)
        {
            Href = url;
            Rel = "self";
        }

        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("href")]
        public string Href { get; set; }
    }
}
