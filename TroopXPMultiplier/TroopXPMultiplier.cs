using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using HarmonyLib;

namespace TroopXPMultiplier
{
    public class Core : MBSubModuleBase
    {
        public static TroopXPMultiplierConfig config = new TroopXPMultiplierConfig();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = true;
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.bannerlord.troopxpmultiplier");
            try
            {
                harmony.PatchAll();
            } catch (Exception e)
            {
                FileLog.Log("TroopXPMultiplier patcher failed! " + e.Message);
            }
        }
    }
}