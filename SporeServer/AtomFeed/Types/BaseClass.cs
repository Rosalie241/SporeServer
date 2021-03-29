using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Types
{
    public class BaseClass
    {
        public BaseClass()
        {
            this.Namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                // TODO, use actual current URL?
                new XmlQualifiedName("sp", "http://spore.com/atom"),
            });
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get; set; }
    }
}
