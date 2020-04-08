using System;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using HarmonyLib;
using System.IO;
using System.Xml;

namespace WarAttrition
{
    public class Core : MBSubModuleBase
    {
        public static WarAttritionConfig config = new WarAttritionConfig();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = false;
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.bannerlord.warattrition");
            harmony.PatchAll();
        }
    }
}
