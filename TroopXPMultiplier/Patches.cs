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
                float simMultiplier = float.Parse(config.SelectSingleNode("SimMultiplier").InnerText);
                float activeMultiplier = 0;
                bool debugText = false;
                bool isThisAPlayer = false;
                int num = attackedTroop.MaxHitPoints();
                xpAmount = MBMath.Round(0.4f * ((attackedTroop.GetPower() + 0.5f) * (float)(Math.Min(damage, num) + (isFatal ? num : 0))));

                if (attackerTroop.HeroObject != null)
                {
                    if (attackerTroop.HeroObject.Equals(Hero.MainHero))
                    {
                        isThisAPlayer = true;
                    }
                }

                if (!isThisAPlayer)
                {
                    float originalXPAmount = xpAmount;
                    if (missionType == CombatXpModel.MissionTypeEnum.SimulationBattle)
                    {
                        xpAmount = MathF.Round((float)xpAmount * simMultiplier);
                        activeMultiplier = simMultiplier;
                    }
                    else if (missionType == CombatXpModel.MissionTypeEnum.PracticeFight)
                    {
                        xpAmount = MathF.Round((float)xpAmount * 0.0625f);
                        activeMultiplier = 0.0625f;
                    }
                    else if (missionType == CombatXpModel.MissionTypeEnum.Tournament)
                    {
                        xpAmount = MathF.Round((float)xpAmount * 0.25f);
                        activeMultiplier = 0.25f;
                    }
                    else
                    {
                        xpAmount = MathF.Round((float)xpAmount * multiplier);
                        activeMultiplier = multiplier;
                    }
                    string nickIsABadTeacher = "Original XP = " + originalXPAmount + "New XP = " + xpAmount + "by " + activeMultiplier + "x";

                    if (debugText == true)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(nickIsABadTeacher));
                    }
                }
                else
                {
                    if (debugText == true)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Player gained XP, no bonus for you, loser"));
                    }
                }
            }
        }
    }
}
