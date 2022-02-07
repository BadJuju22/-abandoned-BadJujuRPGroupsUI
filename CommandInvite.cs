using BadJujuRPGroups.Types;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BadJujuRPGroups
{

    class CommandFInvite : IRocketCommand
    {


        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "finvite";

        public string Help => "";

        public string Syntax => "/finvite <игрок> <фракция> <ранг>";

        public List<string> Aliases => new List<string> { "fi"};

        public List<string> Permissions => new List<string> { "command.finvite" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer inviter = (UnturnedPlayer)caller;
            GroupRP group;
            var uPlayer = (UnturnedPlayer)caller;
            var target = UnturnedPlayer.FromName(command[0]);
            inviter = (UnturnedPlayer)caller;
   
            if(command.Length !=3 || command.Length == 0)
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("syntax_invite"), Color.yellow);
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
            int ranks = Convert.ToInt32(command[2]);
            if (!group.Ranks.Exists(x => x.id == ranks)) 
            {
                UnturnedChat.Say(caller, "Данного ранга не существует", Color.yellow);
                return;
            }
            if (!(caller is ConsolePlayer) || !caller.HasPermission(group.LeadPerm) || !caller.IsAdmin )
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("dont_perm"));
                return;
            }
            if (Plugin.Instance.Configuration.Instance.Fractionplayers.Contains((ulong)target.CSteamID))
            {
                UnturnedChat.Say(caller, Plugin.Instance.Translate("player_in_group"), Color.yellow);
                return;
            }
        
          
           
            EffectManager.sendUIEffect((ushort)Plugin.Instance.Configuration.Instance.Effectid, Plugin.Instance.Configuration.Instance.Effectkey, target.CSteamID, true, Plugin.Instance.Translate("ui_text", caller.DisplayName, group.Name), "Принять", "Отказаться");
        Plugin.Instance.SendGroupRequest((UnturnedPlayer)caller, target, group, ranks);

        }
       
    }
   
}
