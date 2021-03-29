using SporeServer.Areas.Identity.Data;
using SporeServer.AtomFeed.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SporeServer.AtomFeed.Templates.Pollinator
{
    [XmlRoot("handshake", Namespace = "http://www.w3.org/2005/Atom")]
    public class HandshakeTemplate : BaseClass
    {
        public HandshakeTemplate()
        {

        }

        public HandshakeTemplate(SporeServerUser user)
        {
            Author author = new Author()
            {
                Name = user.UserName,
                Uri = "501089776639"
            };

            UserId = 501089776639;
            ScreenName = author.Name;
            NextId = 501089848137;
            //RefreshRate = 1;
            RefreshRate = 120;
            MaxisFeeds = new List<Entry>()
            {
                new Entry()
                {
                    Id = "tag:spore.com,2006:maxis/adventures/en_US",
                    Title = "Maxis Adventures",
                    Updated = DateTime.Now,
                    Subtitle = "Free creations from Maxis.",
                    SubCount = 0,
                    Link = new Link("http://pollinator.spore.com/pollinator/atom/maxis/adventures/en_US")
                }
            };
            MyFeeds = new List<Entry>()
            {
                new Entry()
                {
                    Id = "tag:spore.com,2006:user/501089776639",
                    Title = user.UserName,
                    Updated = DateTime.Now,
                    Author = author,
                    SubCount = 0,
                    Link = new Link("http://pollinator.spore.com/pollinator/atom/user/501089776639")
                }
            };
            InvisibleFeeds = new List<Entry>()
            {
                new Entry()
                {
                    Id = "tag:spore.com,2006:downloadQueue",
                    Title = "Rosalie",
                    Updated = DateTime.Now,
                    Author = author,
                    SubCount = 0,
                    Link = new Link("http://pollinator.spore.com/pollinator/atom/downloadQueue")
                }
            };
        }

        [XmlElement("user-id")]
        public UInt64 UserId { get; set; }

        [XmlElement("screen-name")]
        public string ScreenName { get; set; }

        [XmlElement("next-id")]
        public UInt64 NextId { get; set; }

        [XmlElement("refresh-rate")]
        public int RefreshRate { get; set; }

        [XmlElement("maxis-feeds")]
        public List<Entry> MaxisFeeds { get; set; }

        [XmlElement("my-feeds")]
        public List<Entry> MyFeeds { get; set; }

        [XmlElement("invisible-feeds")]
        public List<Entry> InvisibleFeeds { get; set; }
    }
}
