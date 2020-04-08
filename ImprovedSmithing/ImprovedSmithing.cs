using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System.Collections.Generic;
using HarmonyLib;

namespace ImprovedSmithing
{
    public class Core : MBSubModuleBase
    {
        public static ImprovedSmithingConfig config = new ImprovedSmithingConfig();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = false;
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.bannerlord.improvedsmithing");
            try
            {
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                string str = "Error patching:\n";
                string message = ex.Message;
                string str2 = " \n\n";
                Exception innerException = ex.InnerException;
                FileLog.Log(str + message + str2 + ((innerException != null) ? innerException.Message : null));
            }
        }
    }
}
