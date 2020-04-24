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
    public class ScavengerBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.CharacterDefeated.AddNonSerializedListener(this, new Action<Hero, Hero>(this.ScavengerAction));
        }

        public override void SyncData(IDataStore dataStore)
        {
            //just leave this empty
        }

        //h1 should be the winner, h2 should be the loser
        public void ScavengerAction(Hero h1, Hero h2)
        {
            InformationManager.DisplayMessage(new InformationMessage("SCAVENGER ACTION PROCCED"));
            InformationManager.DisplayMessage(new InformationMessage(h1.Name + " BIG REKT BY " + h2.Name));

            //I was thinking generate the item roster by "buying" a bunch of random items using the amount of gold the lord had and some weird algorithm taking in other factors
            int winnerGold = h1.Gold;
            int loserGold = h2.Gold;
            float winnerFactionStrength = h1.MapFaction.TotalStrength;
            float loserFactionStrength = h2.MapFaction.TotalStrength;

            //gets the position of the heroes, set the winner to the new settlement location
            TaleWorlds.Library.Vec3 h1Pos = h1.GetPosition();
            TaleWorlds.Library.Vec2 h1PosVec2 = h1Pos.AsVec2;
            //TaleWorlds.Library.Vec3 h2Pos = h2.GetPosition();

            TaleWorlds.Localization.TextObject name = new TaleWorlds.Localization.TextObject("Scavenge Me!");

            Scavengeable newScav = new Scavengeable(name, h1PosVec2);
            InformationManager.DisplayMessage(new InformationMessage("SCAVENGEABLE OBJECT SHOULD HAVE BEEN CREATED"));
        }
    }
}
