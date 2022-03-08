using BadJujuRPGroups.Types;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BadJujuRPGroups
{
    class Commandfmanage : IRocketCommand
    {


        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "fmanage";

        public string Help => "";

        public string Syntax => "/fmanage <игрок> <фракция> <ранг>";

        public List<string> Aliases => new List<string> { "fm" };

        public List<string> Permissions => new List<string> { "command.fmanage" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            GroupRP group;
            var uPlayer = (UnturnedPlayer)caller;
            var target = UnturnedPlayer.FromName(command[0]);

         
         
         
          
            if (command.Length != 3 || command.Length == 0)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("syntax_manage"), Color.yellow);
                return;
            }
            if (target == null)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("player_not_found"), Color.yellow);
                return;
            }
            group = Plugin.Instance.Configuration.Instance.GroupsRP.Find(x => x.Name.ToLower() == command[1].ToLower());
            if (group == null)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("group_not_found"), Color.yellow);
                return;
            }
            if (!caller.HasPermission(group.LeadPerm) && !(caller is ConsolePlayer) && !(caller.IsAdmin))
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("dont_perm"));
                return;
            }
            Rank Rank = group.Ranks[Convert.ToInt32(command[2])];
            if (Rank == null)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("rank_not"), Color.yellow);
                return;
            }
            Rank oldrank = group.Ranks.Find(x => x.Members.Contains((ulong)target.CSteamID));
            if (oldrank == null)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("player_in_group"), Color.yellow);
                return;
            }
            int rank = Rank.id;
         R.Permissions.RemovePlayerFromGroup(group.Ranks[(Convert.ToInt32(command[2]))].permid, (IRocketPlayer)target);
            Plugin.Instance.Configuration.Instance.GroupsRP.Find(x => x.Name.ToLower() == command[1].ToLower()).Ranks.Find(x => x.Members.Contains((ulong)target.CSteamID)).Members.Remove((ulong)target.CSteamID);
            if (oldrank.id < Convert.ToInt32(command[2]))
            {
                R.Permissions.AddPlayerToGroup(group.Ranks[Convert.ToInt32(command[2])].permid.ToString(), (IRocketPlayer)uPlayer);
                Plugin.Instance.Configuration.Instance.GroupsRP.Find(x => x.Name.ToLower() == command[1].ToLower()).Ranks[Convert.ToInt32(command[2])].Members.Add((ulong)target.CSteamID);
                UnturnedChat.Say(caller, Plugin.Instance.Translate("rank_up", Rank.id, Rank.name, target.DisplayName));
                UnturnedChat.Say(target, Plugin.Instance.Translate("rank_up_t", Rank.id, Rank.name));
            }
            else if (oldrank.id == Convert.ToInt32(command[2]))
            {
                UnturnedChat.Say(caller, "Зачем менять ранг на тот же самый?", Color.yellow);
            }
            else
            {
              R.Permissions.AddPlayerToGroup(group.Ranks[Convert.ToInt32(command[2])].permid.ToString(), (IRocketPlayer)uPlayer);
                UnturnedChat.Say(caller, Plugin.Instance.Translate("rank_down", Rank.id, Rank.name, target.DisplayName));
                UnturnedChat.Say(target, Plugin.Instance.Translate("rank_down_t", Rank.id, Rank.name));
            }
            Plugin.Instance.Configuration.Save();


        }
    }
}
