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
    public class Scavengeable : MBObjectBase
    {
        private TaleWorlds.Localization.TextObject name;
        private TaleWorlds.Library.Vec2 __position;
        private ItemRoster loot;

        public Scavengeable(TaleWorlds.Localization.TextObject name, TaleWorlds.Library.Vec2 position)
        {
            string debugMessage = "SCAVENGEABLE CREATED WITH NAME " + name.ToString() + " AT " + position.ToString();
            InformationManager.DisplayMessage(new InformationMessage(debugMessage));
            
            this.__position = position;
            this.name = name;
            Campaign.Current.MapSceneWrapper.AddNewEntityToMapScene(name.ToString(), position);
        }
    }
}
