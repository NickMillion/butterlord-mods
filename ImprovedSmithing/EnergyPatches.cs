using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace ImprovedSmithing
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetEnergyCostForRefining")]
    public class AddRefiningPatch
    {
        private static void Postfix(ref Crafting.RefiningFormula refineFormula, Hero hero, ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("EnergySettings");
            int num = int.Parse(config.SelectSingleNode("RefineCost").InnerText);
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalRefiner))
            {
                num = (num + 1) / 2;
            }
            __result = num;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetEnergyCostForSmithing")]
    public class AddSmithingPatch
    {
        private static void Postfix(ItemObject item, Hero hero, ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("EnergySettings");
            int num = int.Parse(config.SelectSingleNode("SmithCost").InnerText);
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalSmith))
            {
                num = (num + 1) / 2;
            }
            __result = num;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetEnergyCostForSmelting")]
    public class AddSmeltingPatch
    {
        private static void Postfix(ItemObject item, Hero hero, ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("EnergySettings");
            int num = int.Parse(config.SelectSingleNode("SmeltCost").InnerText);
            if (hero.GetPerkValue(DefaultPerks.Crafting.PracticalSmelter))
            {
                num = (num + 1) / 2;
            }
            __result = num;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "ResearchPointsNeedForNewPart")]
    public class AddResearchPatch
    {
        private static void Postfix(int count, ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("ResearchSettings");
            int num = int.Parse(config.SelectSingleNode("ResearchCount").InnerText);
            __result = (count * count + 12) / num;
        }
    }

    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetModifierTierForSmithedWeapon")]
    public class AddModifierTierPatch
    {
        private static void Postfix(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel __instance, WeaponDesign weaponDesign, Hero hero, ref int __result)
        {
            //InformationManager.DisplayMessage(new InformationMessage("USING IMPROVED SMITHING PATCH FOR TIER CALC"));
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("ModifierTierSettings");
            float experienceChance = float.Parse(config.SelectSingleNode("FineChance").InnerText);
            float masterChance = float.Parse(config.SelectSingleNode("MasterworkChance").InnerText);
            float legendaryChance = float.Parse(config.SelectSingleNode("LegendaryChance").InnerText);
            bool lowSkillPenalty = bool.Parse(config.SelectSingleNode("LowSkillPenalty").InnerText);

            int num = __instance.CalculateWeaponDesignDifficulty(weaponDesign);
            int num2 = hero.CharacterObject.GetSkillValue(DefaultSkills.Crafting) - num;
            bool tierFound = false;
            if (num2 < 0 && lowSkillPenalty)
            {
                float num0 = MBRandom.RandomFloat;
                num0 += -0.01f * (float)num2;
                if (num0 < 0.4f && !tierFound)
                {
                    __result = -1;
                    tierFound = true;
                }
                if (num0 < 0.7f && !tierFound)
                {
                    __result = -2;
                    tierFound = true;
                }
                if (num0 < 0.9f && !tierFound)
                {
                    __result = -3;
                    tierFound = true;
                }
                if (num0 >= 1f && !tierFound)
                {
                    __result = -5;
                    tierFound = true;
                }
                if (!tierFound)
                {
                    __result = -4;
                    tierFound = true;
                }
            }
            float randomFloat = MBRandom.RandomFloat;
            //experience = 100, master = 200, legend = 275
            if (hero.GetPerkValue(DefaultPerks.Crafting.LegendarySmith) && !tierFound)
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 275)) * (0.1f);
                if (randomFloat < legendaryChance + num3)
                {
                    InformationManager.DisplayMessage(new InformationMessage("LEGENDARY WEAPON CRAFTED"));
                    __result = 3;
                    tierFound = true;
                }
            }
            if (hero.GetPerkValue(DefaultPerks.Crafting.MasterSmith) && !tierFound)
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 200)) * (0.1f);
                if (randomFloat < masterChance + num3)
                {
                    InformationManager.DisplayMessage(new InformationMessage("MASTERWORK WEAPON CRAFTED"));
                    __result = 2;
                    tierFound = true;
                }
            }
            if (hero.GetPerkValue(DefaultPerks.Crafting.ExperiencedSmith) && !tierFound)
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 100)) * (0.1f);
                if (randomFloat < experienceChance + num3)
                {
                    InformationManager.DisplayMessage(new InformationMessage("FINE WEAPON CRAFTED"));
                    __result = 1;
                    tierFound = true;
                }
            }
            if (!tierFound)
            {
                __result = 0;
            }
        }
    }

    //alex is bad, more if statements
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetModifierChanges")]
    public class AddModifierChangesPatch
    {
        private static void Postfix(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel __instance, int modifierTier, ref Crafting.OverrideData __result)
        {
            //InformationManager.DisplayMessage(new InformationMessage("USING IMPROVED SMITHING PATCH FOR TIER BONUS CALC"));
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("ModifierTierSettings");
            float tierBonusMultiplier = float.Parse(config.SelectSingleNode("TierBonusMultiplier").InnerText);
            int tierBonusFlatIncrease = int.Parse(config.SelectSingleNode("TierBonusFlatIncrease").InnerText);

            int pointsToModify = 0;

            if (modifierTier <= -4)
            {
                pointsToModify = -8 + tierBonusFlatIncrease;
            }
            if (modifierTier == -3)
            {
                pointsToModify = -6 + tierBonusFlatIncrease;
            }
            if (modifierTier == -2)
            {
                pointsToModify = -4 + tierBonusFlatIncrease;
            }
            if (modifierTier == -1)
            {
                pointsToModify = -2 + tierBonusFlatIncrease;
            }
            if (modifierTier == 0)
            {
                pointsToModify = 0 + tierBonusFlatIncrease;
            }
            if (modifierTier == 1)
            {
                pointsToModify = (int)(2 * tierBonusMultiplier) + tierBonusFlatIncrease;
            }
            if (modifierTier == 2)
            {
                pointsToModify = (int)(5 * tierBonusMultiplier) + tierBonusFlatIncrease;
            }
            if (modifierTier == 3)
            {
                pointsToModify = (int)(10 * tierBonusMultiplier) + tierBonusFlatIncrease;
            }
            bool returnEmpty = true;
            if (pointsToModify != 0)
            {
                returnEmpty = false;
                Crafting.OverrideData overrideData = new Crafting.OverrideData(0f, 0, 0, 0, 0);
                int num = 0;
                int num2 = 0;
                while (num2 != pointsToModify && num < 2000)
                {
                    int num3 = (pointsToModify > 0) ? 1 : -1;
                    if (MBRandom.RandomFloat < 0.1f)
                    {
                        num3 = -num3;
                    }
                    float randomFloat = MBRandom.RandomFloat;
                    if (randomFloat < 0.2f)
                    {
                        overrideData.SwingSpeedOverriden += num3;
                    }
                    else if (randomFloat < 0.4f)
                    {
                        overrideData.SwingDamageOverriden += num3;
                    }
                    else if (randomFloat < 0.6f)
                    {
                        overrideData.ThrustSpeedOverriden += num3;
                    }
                    else if (randomFloat < 0.8f)
                    {
                        overrideData.ThrustDamageOverriden += num3;
                    }
                    else
                    {
                        overrideData.Handling += num3;
                    }
                    num++;
                    num2 = overrideData.SwingSpeedOverriden + overrideData.SwingDamageOverriden + overrideData.ThrustSpeedOverriden + overrideData.ThrustDamageOverriden + overrideData.Handling;
                }
                __result =  overrideData;
            }
            if (returnEmpty)
            {
                //InformationManager.DisplayMessage(new InformationMessage("NO BONUSES APPLIED TO CRAFT"));
                __result = new Crafting.OverrideData(0f, 0, 0, 0, 0);
            }
        }
    }
}
