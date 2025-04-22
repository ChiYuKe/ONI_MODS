using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElement
{
    public static class SimHashUtil
    {
        public static void RegisterSimHash(string name)
        {
            SimHashes simHashes = (SimHashes)Hash.SDBMLower(name);
            SimHashUtil.SimHashNameLookup.Add(simHashes, name);
            SimHashUtil.ReverseSimHashNameLookup.Add(name, simHashes);
        }


        public static Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();
    }
}
