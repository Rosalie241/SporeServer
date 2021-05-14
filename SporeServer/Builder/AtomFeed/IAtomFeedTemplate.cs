using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Builder.AtomFeed
{
    public interface IAtomFeedTemplate
    {
        /// <summary>
        ///     Serializes the current class to a xml string
        /// </summary>
        /// <returns></returns>
        public string Serialize();
    }
}
