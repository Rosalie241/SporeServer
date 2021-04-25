using SporeServer.Areas.Identity.Data;
using SporeServer.AtomFeed.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Templates.Pollinator.Atom
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class AssetTemplate
    {
        public AssetTemplate()
        {
        }

        public AssetTemplate(SporeServerAsset asset)
        {
           // new Author
        }
    }
}
