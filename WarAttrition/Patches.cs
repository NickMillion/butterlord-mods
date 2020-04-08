using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace WarAttrition
{
    class Patches
    {
        [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior), "ThinkAboutDeclaringPeace")]
        public class AddPatchThinkAboutDeclaringPeace
        {
            private static void Prefix(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior __instance, Kingdom kingdom)
            {
                XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("WarAttritionSettings");
                float raidImpact = float.Parse(config.SelectSingleNode("RaidImpact").InnerText);
                float siegeImpact = float.Parse(config.SelectSingleNode("SiegeImpact").InnerText);
                float casualtyImpact = float.Parse(config.SelectSingleNode("CasualtyImpact").InnerText);
                float attritionNecessary = float.Parse(config.SelectSingleNode("AttritionNecessary").InnerText);
                float superiorMultiplier = float.Parse(config.SelectSingleNode("SuperiorMultiplier").InnerText);
                float inferiorMultiplier = float.Parse(config.SelectSingleNode("InferiorMultiplier").InnerText);
                //double  daysUntilTimeAttrition = int.Parse(config.SelectSingleNode("DaysUntilTimeAttrition").InnerText) * 864000000L;

                List<IFaction> possibleKingdomsToDeclarePeace = FactionHelper.GetPossibleKingdomsToDeclarePeace(kingdom);
                IFaction faction = kingdom.MapFaction;
                float peace = 0f;
                foreach (IFaction faction2 in possibleKingdomsToDeclarePeace)
                {
                    IEnumerable<CampaignWar> enumerable = Campaign.Current.FactionManager.FindCampaignWarsBetweenFactions(faction, faction2);
                    CampaignWar campaignWar;
                    if (enumerable == null)
                    {
                        campaignWar = null;
                    }
                    else
                    {
                        campaignWar = (from war in enumerable.ToList<CampaignWar>()
                                       orderby war.StartDate - CampaignTime.Now descending
                                       select war).FirstOrDefault<CampaignWar>();
                    }
                    CampaignWar campaignWar2 = campaignWar; //because they did this in the base game!

                    double toDays = (CampaignTime.Now - campaignWar2.StartDate).ToDays; //length of war

                    int fac1WarScore = campaignWar2.GetWarScoreOfFaction(faction); //war scores
                    int fac2WarScore = campaignWar2.GetWarScoreOfFaction(faction2);

                    IFaction inferior;
                    IFaction superior;

                    if (fac1WarScore >= fac2WarScore)
                    {
                        superior = faction;
                        inferior = faction2;
                    } else
                    {
                        superior = faction2;
                        inferior = faction;
                    }

                    int superiorRaids = campaignWar2.GetSuccessfulRaidsOfFaction(superior);
                    int inferiorRaids = campaignWar2.GetSuccessfulRaidsOfFaction(inferior);

                    int superiorSieges = campaignWar2.GetSuccessfulSiegesOfFaction(superior);
                    int inferiorSieges = campaignWar2.GetSuccessfulSiegesOfFaction(inferior);

                    int superiorCasualties = campaignWar2.GetCasualtiesOfFaction(superior);
                    int inferiorCasualties = campaignWar2.GetCasualtiesOfFaction(inferior);

                    //double dayPeaceIncrease = 0;
                    /**if (toDays > daysUntilTimeAttrition)
                    {
                        dayPeaceIncrease = ((toDays - daysUntilTimeAttrition) / 864000000L) / 100;
                    }*/

                    float superiorCasualtiesHit = (superiorCasualties / casualtyImpact) / 100;
                    float inferiorCasualtiesHit = (inferiorCasualties / casualtyImpact) / 100;

                    float superiorTotalAttrition = ((superiorRaids * raidImpact) + (superiorSieges * siegeImpact) + superiorCasualtiesHit) * superiorMultiplier;
                    float inferiorTotalAttrition = ((inferiorRaids * raidImpact) + (inferiorSieges * siegeImpact) + inferiorCasualtiesHit) * inferiorMultiplier;

                    peace = superiorTotalAttrition + inferiorTotalAttrition; //+ (float)dayPeaceIncrease;
                    float chance = MBRandom.RandomFloat;
                    if (chance < peace && peace > attritionNecessary) //if peace chance is rolled and peace is greater than attrition minimum
                    {
                        //InformationManager.DisplayMessage(new InformationMessage("PEACE DECLARED BETWEEN " + kingdom.Name.ToString() + " AND " + faction2.Name.ToString() + " WITH " + peace + " ATTRITION."));
                        MakePeaceAction.Apply(kingdom, faction2);
                    }
                }
            }
        }

        /**[HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior), "ThinkAboutDeclaringWar")]
        public class AddPatchThinkAboutDeclaringWar
        {
            private static void Prefix(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior __instance, Kingdom kingdom)
            {
                XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("WarAttritionSettings");
                float daysSinceLastWarStartToStartNew = float.Parse(config.SelectSingleNode("DaysSinceLastWarStartToStartNew").InnerText);
                double daysSinceLastWarStartCheck = 0;
                List<IFaction> possibleKingdomsToDeclareWar = FactionHelper.GetPossibleKingdomsToDeclareWar(kingdom);
                double timeSinceLastWarStart = 0;
                float num = 0f;
                IFaction faction = null;
                foreach (IFaction faction2 in possibleKingdomsToDeclareWar)
                {
                    IEnumerable<CampaignWar> enumerable = Campaign.Current.FactionManager.FindCampaignWarsBetweenFactions(faction, faction2);
                    CampaignWar campaignWar;
                    if (enumerable == null)
                    {
                        campaignWar = null;
                    }
                    else
                    {
                        campaignWar = (from war in enumerable.ToList<CampaignWar>()
                                       orderby war.StartDate - CampaignTime.Now descending
                                       select war).FirstOrDefault<CampaignWar>();
                    }
                    CampaignWar campaignWar2 = campaignWar; //because they did this in the base game!

                    float scoreOfDeclaringWar = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(kingdom, faction2, false);
                    if (scoreOfDeclaringWar > num)
                    {

                        timeSinceLastWarStart = (CampaignTime.Now - campaignWar2.StartDate).ToDays;
                        daysSinceLastWarStartCheck = CampaignTime.DaysFromNow(daysSinceLastWarStartToStartNew).ToDays;
                        faction = faction2;
                        num = scoreOfDeclaringWar;
                    }
                }
                if (faction != null && MBRandom.RandomFloat < Math.Min(0.25f, num / 100000f) && timeSinceLastWarStart > daysSinceLastWarStartCheck)
                {
                    DeclareWarAction.ApplyDeclareWarOverProvocation(kingdom, faction);
                }
            }
        }*/


        [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior), "DailyTick")]
        public class AddDailyTickPatch
        {
            private static void Prefix(TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.PoliticalStagnationAndBorderIncidentCampaignBehavior __instance)
            {
                XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("WarAttritionSettings");
                float dailyCheckChance = float.Parse(config.SelectSingleNode("DailyCheckChance").InnerText);
                bool playerLeadKingdomIgnoresAutoPeace = bool.Parse(config.SelectSingleNode("PlayerLeadKingdomIgnoresAutoPeace").InnerText);

                foreach (Kingdom kingdom in Kingdom.All)
                {
                    bool flag = false;
                    foreach (Kingdom kingdom2 in Kingdom.All)
                    {
                        if (FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2) && kingdom2.Fortifications.Count<Town>() >= 3)
                        {
                            kingdom.PoliticallyStagnation--;
                            if (kingdom.PoliticallyStagnation < 0)
                            {
                                kingdom.PoliticallyStagnation = 0;
                            }
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        kingdom.PoliticallyStagnation++;
                    };
                }
                foreach (Hero hero in Hero.All)
                {
                    if (hero.CharacterObject.Occupation == Occupation.Lady || hero.CharacterObject.Occupation == Occupation.Lord)
                    {
                        hero.Controversy *= 0.99f;
                    }
                }
                foreach (Kingdom kingdom2 in Kingdom.All)
                {
                    if (MBRandom.RandomFloat < dailyCheckChance && kingdom2.IsKingdomFaction)
                    {
                        if (!TaleWorlds.CampaignSystem.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.CampaignSystem.ManagedParametersEnum.IsWarDeclarationDisabled))
                        {
                            __instance.ThinkAboutDeclaringWar(kingdom2);
                        }
                        if (!TaleWorlds.CampaignSystem.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.CampaignSystem.ManagedParametersEnum.IsPeaceDeclarationDisabled))
                        {
                            if (playerLeadKingdomIgnoresAutoPeace)
                            {
                                if (!kingdom2.Leader.Equals(Hero.MainHero))
                                {
                                    __instance.ThinkAboutDeclaringPeace(kingdom2);
                                }
                            } else
                            {
                                __instance.ThinkAboutDeclaringPeace(kingdom2);
                            }
                        }
                    }
                }
            }
        }
    }
}
