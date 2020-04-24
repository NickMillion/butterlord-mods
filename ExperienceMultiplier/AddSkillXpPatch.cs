using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace ExperienceMultiplier
{
    [HarmonyPatch(typeof(Hero), "AddSkillXp")]
    public class AddSkillXpPatcher
    {
        private static void Prefix(Hero __instance, SkillObject skill, float xpAmount)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("MultiplierSettings");

            //Just making it a little easier to remember
            Hero thisHeroRef = __instance;
            Hero mainHeroRef = Hero.MainHero;

            HeroDeveloper heroDeveloper = __instance.HeroDeveloper;
            if (heroDeveloper != null && skill != null)
            {

                #region loading stuff
                bool linearLeveling = bool.Parse(config.SelectSingleNode("LinearLeveling").InnerText);
                bool onlyMain = bool.Parse(config.SelectSingleNode("OnlyPlayerHero").InnerText);
                bool alsoCompanions = bool.Parse(config.SelectSingleNode("AlsoPlayerCompanions").InnerText);
                float vigMultiplier = float.Parse(config.SelectSingleNode("VIGMultiplier").InnerText);
                float ctrMultiplier = float.Parse(config.SelectSingleNode("CTRMultiplier").InnerText);
                float endMultiplier = float.Parse(config.SelectSingleNode("ENDMultiplier").InnerText);
                float cngMultiplier = float.Parse(config.SelectSingleNode("CNGMultiplier").InnerText);
                float socMultiplier = float.Parse(config.SelectSingleNode("SOCMultiplier").InnerText);
                float intMultiplier = float.Parse(config.SelectSingleNode("INTMultiplier").InnerText);
                #endregion

                //Initialize the final multiplier as base multiplier
                float finalMultiplier = float.Parse(config.SelectSingleNode("Multiplier").InnerText);

                //should add a bool to skip over all the skill multipliers? "EnableSkillMultipliers"?
                #region skills and stuff
                #region vigor
                //vig
                if (skill.GetName().Equals(DefaultSkills.OneHanded.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("OneHandedMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * vigMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.TwoHanded.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("TwoHandedMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * vigMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Polearm.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("PolearmMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * vigMultiplier;
                }
                #endregion

                #region control
                //ctr
                if (skill.GetName().Equals(DefaultSkills.Bow.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("BowMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * ctrMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Crossbow.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("CrossbowMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * ctrMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Throwing.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("ThrowingMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * ctrMultiplier;
                }
                #endregion

                #region endurance
                //end
                if (skill.GetName().Equals(DefaultSkills.Riding.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("RidingMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * endMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Athletics.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("AthleticsMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * endMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Crafting.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("SmithingMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * endMultiplier;
                }
                #endregion

                #region cunning
                //cng
                if (skill.GetName().Equals(DefaultSkills.Scouting.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("ScoutingMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * cngMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Tactics.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("TacticsMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * cngMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Roguery.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("RogueryMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * cngMultiplier;
                }
                #endregion

                #region social
                //soc
                if (skill.GetName().Equals(DefaultSkills.Charm.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("CharmMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * socMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Leadership.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("LeadershipMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * socMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Trade.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("TradeMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * socMultiplier;
                }
                #endregion

                #region intellect
                //int
                if (skill.GetName().Equals(DefaultSkills.Steward.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("StewardMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * intMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Medicine.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("MedicineMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * intMultiplier;
                }
                if (skill.GetName().Equals(DefaultSkills.Engineering.GetName()))
                {
                    float thisMultiplier = float.Parse(config.SelectSingleNode("EngineeringMultiplier").InnerText);
                    finalMultiplier *= thisMultiplier * intMultiplier;
                }
                #endregion
                #endregion

                #region linear leveling
                if (linearLeveling)
                {
                    float perSkillLevelMultiplier = float.Parse(config.SelectSingleNode("MultiplierIncreasePerSkillLevel").InnerText);
                    float linearMultiplier = 1 + (thisHeroRef.GetSkillValue(skill) * perSkillLevelMultiplier);
                    finalMultiplier *= linearMultiplier;
                }
                #endregion
                
                //Initialize how much XP we'll be adding.
                float xpToAdd = xpAmount;

                //if (thisHeroRef.Equals(mainHeroRef))
                //{
                //    InformationManager.DisplayMessage(new InformationMessage("ORIGINAL XP TO BE ADDED TO " + thisHeroRef.Name + " = " + xpToAdd + " WITH MULTIPLIER OF " + finalMultiplier));
                //}

                #region only main or companions check
                if (onlyMain)
                {
                    if (thisHeroRef.Equals(mainHeroRef))
                    {
                        float playerMultiplier = float.Parse(config.SelectSingleNode("PlayerHeroMultiplier").InnerText);
                        xpToAdd *= playerMultiplier;
                    }
                    else if (alsoCompanions && thisHeroRef.Clan.Equals(mainHeroRef.Clan))
                    {
                        float companionMultiplier = float.Parse(config.SelectSingleNode("PlayerCompanionsMultiplier").InnerText);
                        xpToAdd *= companionMultiplier;
                    }
                }
                #endregion
                
                float maxXpDrop = float.Parse(config.SelectSingleNode("MaxXpPerEvent").InnerText);

                xpToAdd *= finalMultiplier;
                xpToAdd = Math.Min(xpToAdd, maxXpDrop);
                heroDeveloper.AddSkillXp(skill, xpToAdd, true, true);

                //if (thisHeroRef.Equals(mainHeroRef))
                //{
                //    InformationManager.DisplayMessage(new InformationMessage("TOTAL XP TO BE ADDED TO " + thisHeroRef.Name + " = " + xpToAdd + " WITH MULTIPLIER OF " + finalMultiplier));
                //}
            }
        }
    }
}