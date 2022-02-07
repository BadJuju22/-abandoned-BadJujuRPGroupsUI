using BadJujuRPGroups.Types;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadJujuRPGroups
{
    public class Plugin : RocketPlugin<Config>
    {
        public override TranslationList DefaultTranslations => new TranslationList
        {
         
            { "player_in_group", "Игрок уже находится в какой-либо группе" },
            { "player_not_found", "Данный игрок не найден" },
            { "group_not_found", "Данная группа не найден." },
            {"ui_text", "{0} предлагает вам вступить в {1}" },
            { "dont_perm", "У вас нет прав для совершения данного действия" },
            { "rank_up", "Вы повысили до ранга {0} [{1}] игрока {2} " },
            { "rank_up_t", "Вас повысили до ранга {0} [{1}]" },
            { "rank_down", "Вы понизили до ранга {0} [{1}] игрока {2}" },
            { "rank_down_t", "Вас понизили до ранга {0}" },
            { "can_add_disabled", "Вы не можете добавлять в данную группу" },
            { "add_player_to_group", "Вы добавили игрока: [{0}] в группу: [{1}]" },
            { "add_player_to_group_t", "Вас добавили в группу: [{1}]" },
            {"decline_request", "Ваш запрос был отклонен"},
            {"decline_request_t", "Вы отклонили запрос" },
            {"duplicate", "Вы уже отправили запрос" },
            { "remove_from_group", "Вы удалили игрока: [{0}] из группы: [{1}]" },
            { "remove_from_group_t", "Вас исключили из группы: [{1}]" },
            {"rank_not", "Данного ранга не существует" },
            {"syntax_invite", "/finvite <игрок> <фракция> <ранг>" },
            {"syntax_manage", "/fmanage <игрок> <фракция> <ранг>" },
            {"syntax_kick", "/fkick <игрок> <фракция>" },
            {"yourself", "Вы не можете отправить запрос самому себе!" },


        };

        public static Plugin Instance { get; private set; }
        public Color MessageColor { get; internal set; }
        public List<GroupRequest> GroupRequests { get; set; }
  

        protected override void Load()
        {
            Console.WriteLine("Plguin was made by BadJuju, if you have any questions write in Discord BadJuju#8608", Color.green);
            Instance = this;        
            EffectManager.onEffectButtonClicked += OnButtonClicked;        
        }

      
        private void OnButtonClicked(Player player, string buttonName)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            var request = GroupRequests.FirstOrDefault(x => x.Target == uPlayer.CSteamID);
            if (buttonName == "acceptgroup")
            {

            
                UnturnedPlayer targ = UnturnedPlayer.FromCSteamID(request.Target);
            R.Permissions.AddPlayerToGroup(request.Group.Ranks[request.Rank].permid.ToString(), (IRocketPlayer)uPlayer);
             R.Permissions.AddPlayerToGroup(request.Group.Perm.ToString(), (IRocketPlayer)uPlayer);
                Configuration.Instance.GroupsRP.Find(x => x.Name == request.Group.Name).Ranks[request.Rank].Members.Add((ulong)request.Target);
                Configuration.Instance.Fractionplayers.Add((ulong)request.Target);
               
                
                GroupRequestHelper.ClearPlayerRequests(Plugin.Instance, uPlayer.CSteamID);
                UnturnedChat.Say(request.Sender, Translate("add_player_to_group", targ.DisplayName, request.Group.Name), Color.yellow);
                UnturnedChat.Say(request.Target, Translate("add_player_to_group_t", request.Group.Name), Color.yellow);
                Configuration.Save();
            }
            else if(buttonName == "declinegroup")
            {
              request = GroupRequests.FirstOrDefault(x => x.Target == uPlayer.CSteamID);

                GroupRequestHelper.ClearPlayerRequests(Plugin.Instance, uPlayer.CSteamID);
                UnturnedChat.Say(request.Sender, Translate("decline_request"), Color.yellow);
               UnturnedChat.Say(request.Target, Translate("decline_request_t"), Color.yellow);


                }
                    
              
            }
        

       
        protected override void Unload()
        {
            EffectManager.onEffectButtonClicked -= OnButtonClicked;
        }

    }
    }
