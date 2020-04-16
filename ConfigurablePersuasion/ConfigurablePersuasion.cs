using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System.Collections.Generic;
using HarmonyLib;

namespace ConfigurablePersuasion
{
    public class Core : MBSubModuleBase
    {
        public static ConfigurablePersuasionConfig config = new ConfigurablePersuasionConfig();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = true; //If true then this will probably generate a log on your desktop
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.bannerlord.configurablepersuasion");
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
