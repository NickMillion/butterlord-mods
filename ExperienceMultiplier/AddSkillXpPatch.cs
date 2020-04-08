using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace ExperienceMultiplier
{
    [HarmonyPatch(typeof(Hero), "AddSkillXp")]
    public class AddSkillXpPatcher
    {
        private static void Prefix(Hero __instance, SkillObject skill, int xpAmount)
        {
            try
            {
                XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("MultiplierSettings");
                HeroDeveloper heroDeveloper = (HeroDeveloper)typeof(Hero).GetField("_heroDeveloper", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                Hero thisHeroRef = __instance;
                Hero mainHeroRef = Hero.MainHero;
                bool flag = heroDeveloper != null;
                if (flag)
                {
                    bool onlyMain = bool.Parse(config.SelectSingleNode("OnlyPlayerHero").InnerText);
                    bool alsoCompanions = bool.Parse(config.SelectSingleNode("AlsoPlayerCompanions").InnerText);
                    double vigMultiplier = double.Parse(config.SelectSingleNode("VIGMultiplier").InnerText);
                    double ctrMultiplier = double.Parse(config.SelectSingleNode("CTRMultiplier").InnerText);
                    double endMultiplier = double.Parse(config.SelectSingleNode("ENDMultiplier").InnerText);
                    double cngMultiplier = double.Parse(config.SelectSingleNode("CNGMultiplier").InnerText);
                    double socMultiplier = double.Parse(config.SelectSingleNode("SOCMultiplier").InnerText);
                    double intMultiplier = double.Parse(config.SelectSingleNode("INTMultiplier").InnerText);

                    double baseMultiplier = double.Parse(config.SelectSingleNode("Multiplier").InnerText); //multiplier
                    double finalMultiplier = baseMultiplier;

                    //vig
                    if (skill.GetName().Equals(DefaultSkills.OneHanded.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("OneHandedMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * vigMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.TwoHanded.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("TwoHandedMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * vigMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Polearm.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("PolearmMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * vigMultiplier;
                    }

                    //ctr
                    if (skill.GetName().Equals(DefaultSkills.Bow.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("BowMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * ctrMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Crossbow.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("CrossbowMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * ctrMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Throwing.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("ThrowingMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * ctrMultiplier;
                    }

                    //end
                    if (skill.GetName().Equals(DefaultSkills.Riding.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("RidingMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * endMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Athletics.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("AthleticsMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * endMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Crafting.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("SmithingMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * endMultiplier;
                    }

                    //cng
                    if (skill.GetName().Equals(DefaultSkills.Scouting.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("ScoutingMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * cngMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Tactics.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("TacticsMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * cngMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Roguery.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("RogueryMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * cngMultiplier;
                    }

                    //soc
                    if (skill.GetName().Equals(DefaultSkills.Charm.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("CharmMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * socMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Leadership.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("LeadershipMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * socMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Trade.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("TradeMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * socMultiplier;
                    }

                    //int
                    if (skill.GetName().Equals(DefaultSkills.Steward.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("StewardMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * intMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Medicine.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("MedicineMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * intMultiplier;
                    }
                    if (skill.GetName().Equals(DefaultSkills.Engineering.GetName()))
                    {
                        double thisMultiplier = double.Parse(config.SelectSingleNode("EngineeringMultiplier").InnerText);
                        finalMultiplier = baseMultiplier * thisMultiplier * intMultiplier;
                    }
                    int num = 0;
                    if (onlyMain)
                    {
                        if (alsoCompanions)
                        {
                            if (thisHeroRef.Equals(mainHeroRef)) //main hero
                            {
                                double thisMultiplier = double.Parse(config.SelectSingleNode("PlayerHeroMultiplier").InnerText);
                                num = (int)Math.Ceiling((double)xpAmount * finalMultiplier * thisMultiplier);
                                heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                            }
                            else if (thisHeroRef.Clan.Equals(mainHeroRef.Clan) && !thisHeroRef.Equals(mainHeroRef)) //companions but not main hero
                            {
                                double thisMultiplier = double.Parse(config.SelectSingleNode("PlayerCompanionsMultiplier").InnerText);
                                num = (int)Math.Ceiling((double)xpAmount * finalMultiplier * thisMultiplier);
                                heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                            }
                            else //everyone else
                            {
                                num = (int)Math.Ceiling((double)xpAmount);
                                heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                            }
                        }
                        else
                        {
                            if (thisHeroRef.Equals(mainHeroRef))
                            {
                                double thisMultiplier = double.Parse(config.SelectSingleNode("PlayerHeroMultiplier").InnerText);
                                num = (int)Math.Ceiling((double)xpAmount * finalMultiplier * thisMultiplier);
                                heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                            }
                            else
                            {
                                num = (int)Math.Ceiling((double)xpAmount);
                                heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                            }
                        }
                    } else
                    {
                        if (thisHeroRef.Equals(mainHeroRef)) //main hero
                        {
                            double thisMultiplier = double.Parse(config.SelectSingleNode("PlayerHeroMultiplier").InnerText);
                            num = (int)Math.Ceiling((double)xpAmount * finalMultiplier * thisMultiplier);
                            heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                        }
                        else if (thisHeroRef.Clan.Equals(mainHeroRef.Clan) && !thisHeroRef.Equals(mainHeroRef)) //companions but not main hero
                        {
                            double thisMultiplier = double.Parse(config.SelectSingleNode("PlayerCompanionsMultiplier").InnerText);
                            num = (int)Math.Ceiling((double)xpAmount * finalMultiplier * thisMultiplier);
                            heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                        }
                        else //everyone else
                        {
                            num = (int)Math.Ceiling((double)xpAmount * finalMultiplier);
                            heroDeveloper.AddSkillXp(skill, (float)num, true, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                FileLog.Log("Patcher " + e.Message);
            }
        }
    }
}