using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

namespace ImprovedSmithing
{
    public class ImprovedSmithingModel : DefaultSmithingModel
    {
        public override int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero)
        {
            return Core.refiningCost;
        }

        public override int GetEnergyCostForSmithing(ItemObject item, Hero hero)
        {
            return Core.smithingCost;
        }

        public override int GetEnergyCostForSmelting(ItemObject item, Hero hero)
        {
            return Core.smeltingCost;
        }

        public override int ResearchPointsNeedForNewPart(int count)
        {
            return (count * count + 12) / Core.researchCount;
        }

        /**public override int GetModifierTierForSmithedWeapon(WeaponDesign weaponDesign, Hero hero)
        {
            int num = this.CalculateWeaponDesignDifficulty(weaponDesign);
            int num2 = hero.CharacterObject.GetSkillValue(DefaultSkills.Crafting) - num;
            if (num2 < 0 && Core.lowSkillPenalty)
            {
                return this.GetPenaltyForLowSkill(num2);
            }
            float randomFloat = MBRandom.RandomFloat;
            Random randomBonus = new Random();
            float r = randomBonus.Next(Core.statTierBonusMin, Core.statTierBonusMax);
            //experience = 100, master = 200, legend = 275
            if (hero.GetPerkValue(DefaultPerks.Crafting.LegendarySmith))
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 275)) * (0.1f + (1 * Core.rarityBonus));
                if (randomFloat < Core.legendaryChance + num3)
                {
                    return 3 + (int) r;
                }
            }
            if (hero.GetPerkValue(DefaultPerks.Crafting.MasterSmith))
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 200)) * (0.1f + (1 * Core.rarityBonus));
                if (randomFloat < Core.masterChance + num3)
                {
                    r = r * 0.5f;
                    return 2 + (int) r;
                }
            }
            if (hero.GetPerkValue(DefaultPerks.Crafting.ExperiencedSmith))
            {
                float num3 = Math.Max(0f, (float)(hero.GetSkillValue(DefaultSkills.Crafting) - 100)) * (0.1f + (1 * Core.rarityBonus));
                if (randomFloat < Core.experiencedChance + num3)
                {
                    r = r * 0.25f;
                    return 1 + (int) r;
                }
            }
            return 0;
        }

        private int GetPenaltyForLowSkill(int difference)
        {
            float num = MBRandom.RandomFloat;
            num += -0.01f * (float)difference;
            if (num < 0.4f)
            {
                return -1;
            }
            if (num < 0.7f)
            {
                return -2;
            }
            if (num < 0.9f)
            {
                return -3;
            }
            if (num >= 1f)
            {
                return -5;
            }
            return -4;
        }*/
    }
}