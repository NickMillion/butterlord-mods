using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using HarmonyLib;
using System.IO;
using System.Xml;

namespace Scavenger
{
    public class Core : MBSubModuleBase
    {

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            InformationManager.DisplayMessage(new InformationMessage("SCAVENGER LOADED MAIN MENU"));
            //config stuff goes here
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                InformationManager.DisplayMessage(new InformationMessage("SCAVENGER LOADED IN GAME"));

                CampaignGameStarter starter = (CampaignGameStarter)gameStarterObject;
                starter.AddBehavior(new ScavengerBehavior());
                base.OnGameStart(game, starter);
                //Need to add a listener that procs an action at the end of every battle
                //The action needs to spawn some sort of map interactable object (an immobile party or temporary settlement? look for camping examples)
                //Custom behavior for "raiding" this object with chance of spawning defender party

                //map event started and/or ended?
                //prisoner taken event?
                //character defeated?
                //look at cant run forever mod for examples on adding new behaviors and consider looking into game models stuff as well


            }
        }
        
    }
}
