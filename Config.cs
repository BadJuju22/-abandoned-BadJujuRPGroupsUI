using BadJujuRPGroups.Types;
using Rocket.API;
using System.Collections.Generic;

namespace BadJujuRPGroups
{
    public class Config : IRocketPluginConfiguration
    {
        public short Effectid = 20435;
        public short Effectkey = 20435;
        public List<GroupRP> GroupsRP;
        public List<ulong> Fractionplayers;
        
        public void LoadDefaults()
        {
            GroupsRP = new List<GroupRP>
            {
                new GroupRP
                {
                    Name = "Полиция",
                    Perm = "police",
                  
                    LeadPerm = "gpol",
                    Ranks = new List<Rank>{
                        new Rank
                        {
                            id = 0,
                            name = "генерал",
                            permid = "police0",
                            Members = new List<ulong>
                            {
                                76583823832234,
                                76582211212224,
                            }

                        }

                    },

                }
            };
            Fractionplayers = new List<ulong>
            {
                765358345842,

            };

        }
    }
}