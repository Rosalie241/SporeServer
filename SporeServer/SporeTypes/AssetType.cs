using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.SporeTypes
{
    public enum AssetType : Int64
    {
        Creature = 0x2b978c46,
        Building = 0x2399be55,
        Vehicle = 0x24682294,
        Ufo = 0x476a98c7,
        Adventure = 0x366a930d
    };
}
