using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace BadJujuRPGroups.Types
{
    public class GroupRP
    {
        public string Perm { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
      
        public string LeadPerm;
       
        [XmlArray]
       
        public List<Rank> Ranks;


    }
 
    public class Rank
    {
        public int id;
        public string name;
        public string permid;
        public List<ulong> Members;

    }
    public class GroupRequest
    {
        public GroupRequest(CSteamID sender, CSteamID target, GroupRP group, int rank)
        {
            Sender = sender;
            Target = target;
            Group = group;
            Rank = rank;
        }

        public GroupRequest() { }

        
        public CSteamID Sender { get; set; }
        public CSteamID Target { get; set; }
        public GroupRP Group { get; set;  }
        public int Rank { get; set; }
        public UnturnedPlayer SenderPlayer => UnturnedPlayer.FromCSteamID(Sender);
        public UnturnedPlayer TargetPlayer => UnturnedPlayer.FromCSteamID(Target);
    }
        public static class GroupRequestHelper
    {
        public static void SendGroupRequest(this Plugin plugin, UnturnedPlayer sender, UnturnedPlayer target, GroupRP group, int rank)
        {
            if (sender.Id == target.Id)
            {
                UnturnedChat.Say(sender, plugin.Translate("yourself"), Color.red);
                return;
            }

           
            

            if (plugin.GroupRequests.Exists(x => x.Sender == sender.CSteamID && x.Target == target.CSteamID))
            {
                UnturnedChat.Say(sender, plugin.Translate("duplicate"), plugin.MessageColor);
                return;
            }

            var request = new GroupRequest(sender.CSteamID, target.CSteamID, group, rank);
           
            

            plugin.GroupRequests.Add(request);
           

           
        }

      


        public static void ClearPlayerRequests(this Plugin plugin, CSteamID steamID)
        {
            plugin.GroupRequests.RemoveAll(x => x.Sender == steamID || x.Target == steamID);
        }
    }

}

