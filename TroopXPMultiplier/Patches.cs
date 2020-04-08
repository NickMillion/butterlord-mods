using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TroopXPMultiplier
{
    class Patches
    {
        [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultCombatXpModel), "GetXpFromHit")]
        public class GetXpFromHitPatch
        {
            private static void Prefix(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultCombatXpModel __instance, CharacterObject attackerTroop, CharacterObject attackedTroop, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType, out int xpAmount)
            {
                XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("TroopXPSettings");
                float multiplier = float.Parse(config.SelectSingleNode("Multiplier").InnerText);

                int num = attackedTroop.MaxHitPoints();
                xpAmount = MBMath.Round(0.4f * ((attackedTroop.GetPower() + 0.5f) * (float)(Math.Min(damage, num) + (isFatal ? num : 0))));
                if (missionType == CombatXpModel.MissionTypeEnum.SimulationBattle)
                {
                    xpAmount *= 8;
                }
                if (missionType == CombatXpModel.MissionTypeEnum.PracticeFight)
                {
                    xpAmount = MathF.Round((float)xpAmount * 0.0625f);
                }
                if (missionType == CombatXpModel.MissionTypeEnum.Tournament)
                {
                    xpAmount = MathF.Round((float)xpAmount * 0.25f);
                }
                xpAmount = MathF.Round((float)xpAmount * multiplier);
            }
        }
    }
}
