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
    public class Actions
    {
        public static void TestAction(Hero h1, Hero h2)
        {
            InformationManager.DisplayMessage(new InformationMessage(h1.Name + " BIG REKT BY " + h2.Name));
        }
    }
}
