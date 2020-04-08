using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System.Collections.Generic;
using HarmonyLib;
using System.IO;
using System.Xml;

namespace ExperienceMultiplier
{
    public class Core : MBSubModuleBase
    {
        public static ExperienceMultiplierConfig config = new ExperienceMultiplierConfig();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = false;
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.bannerlord.experiencemultiplier");
            harmony.PatchAll();
            foreach ((MethodBase original, MethodInfo prefix, MethodInfo postfix) patch in config.toPatch)
            {
                    try
                    {
                        if (patch.original == null)
                            continue;
                        if (patch.prefix == null)
                            harmony.Patch(patch.original, postfix: new HarmonyMethod(patch.postfix));
                        else if (patch.postfix == null)
                            harmony.Patch(patch.original, prefix: new HarmonyMethod(patch.prefix));
                        else
                            harmony.Patch(patch.original, postfix: new HarmonyMethod(patch.postfix), prefix: new HarmonyMethod(patch.prefix));
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
}
