using SporeServer.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Types
{
    public class Author
    {
        public Author()
        {

        }

        public Author(SporeServerUser user)
        {
            Name = user.UserName;
            Uri = user.Id.ToString();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("uri")]
        public string Uri { get; set; }
    }
}
