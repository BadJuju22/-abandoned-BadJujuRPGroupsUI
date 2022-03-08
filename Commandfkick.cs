using BadJujuRPGroups.Types;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnityEngine;

namespace BadJujuRPGroups
{

    class CommandFkick : IRocketCommand
    {


        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "fkick";

        public string Help => "";

        public string Syntax => "/fkick <игрок> <фракция>";

        public List<string> Aliases => new List<string> { "fk"};

        public List<string> Permissions => new List<string> { "command.fkick" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            GroupRP group;
            var uPlayer = (UnturnedPlayer)caller;
            var target = UnturnedPlayer.FromName(command[0]);
          
            
         
            if (command.Length != 2 || command.Length == 0)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("kick_syntax"), Color.yellow);
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
            Rank Rank = group.Ranks.Find(x => x.Members.Contains((ulong)target.CSteamID));
            if (Rank == null)
            {
                UnturnedChat.Say("Игрок не состоит во фракции", Color.yellow);
                return;
            }
           R.Permissions.RemovePlayerFromGroup(group.Ranks.Find(x => x.Members.Contains((ulong)target.CSteamID)).permid.ToString(), (IRocketPlayer)target);
           R.Permissions.RemovePlayerFromGroup(group.Perm.ToString(), (IRocketPlayer)target);
            Plugin.Instance.Configuration.Instance.Fractionplayers.Remove((ulong)target.CSteamID);
            Plugin.Instance.Configuration.Instance.GroupsRP.Find(x => x.Name.ToLower() == command[1].ToLower()).Ranks.Find(x => x.Members.Contains((ulong)target.CSteamID)).Members.Remove((ulong)target.CSteamID);
            Plugin.Instance.Configuration.Save();
            UnturnedChat.Say(caller, Plugin.Instance.Translate("remove_from_group", target.DisplayName, group.Name), Color.red);
            UnturnedChat.Say(target, Plugin.Instance.Translate("remove_from_group_t", group.Name), Color.red);


        }
    }
}
