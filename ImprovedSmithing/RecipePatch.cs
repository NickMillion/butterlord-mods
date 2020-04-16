using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using HarmonyLib;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using System.Xml;

namespace ImprovedSmithing
{
    [HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.Map.DefaultSmithingModel), "GetRefiningFormulas")]
    class RecipePatch
    {
        private static void Postfix(DefaultSmithingModel __instance, Hero weaponsmith, ref IEnumerable<Crafting.RefiningFormula> __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("RecipeSettings");
            int normalCharcoalOutput = int.Parse(config.SelectSingleNode("NormalCharcoalOutput").InnerText);
            int efficientCharcoalOutput = int.Parse(config.SelectSingleNode("EfficientCharcoalOutput").InnerText);

            List<Crafting.RefiningFormula> list = new List<Crafting.RefiningFormula>();
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.Wood, 2, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, normalCharcoalOutput, CraftingMaterials.IronOre, 0));
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Wood, 3, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, efficientCharcoalOutput, CraftingMaterials.IronOre, 0));
            }
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.IronOre, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? 3 : 2, CraftingMaterials.IronOre, 0));
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron1, 1, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron2, 1, CraftingMaterials.IronOre, 0));
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron2, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron3, 1, CraftingMaterials.Iron1, 1));
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron3, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron4, 1, CraftingMaterials.Iron1, 1));
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron4, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron5, 1, CraftingMaterials.Iron1, 1));
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron5, 2, CraftingMaterials.Charcoal, 1, CraftingMaterials.Iron6, 1, CraftingMaterials.Iron1, 1));
            }
            
            int bulkCharcoalOutput = int.Parse(config.SelectSingleNode("BulkCharcoalOutput").InnerText);

            //bulk recipes
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.CharcoalMaker)) //bulk charcoal
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Wood, 10, CraftingMaterials.Iron1, 0, CraftingMaterials.Charcoal, bulkCharcoalOutput, CraftingMaterials.IronOre, 0));
            }

            int bulkMaterialInput = int.Parse(config.SelectSingleNode("BulkRecipeMaterialInput").InnerText);
            int bulkCharcoalInput = int.Parse(config.SelectSingleNode("BulkRecipeCharcoalInput").InnerText);
            int bulkPrimaryOutput = int.Parse(config.SelectSingleNode("BulkRecipePrimaryOutput").InnerText);
            int bulkSecondaryOutput = int.Parse(config.SelectSingleNode("BulkRecipeSecondaryOutput").InnerText);

            list.Add(new Crafting.RefiningFormula(CraftingMaterials.IronOre, bulkMaterialInput, CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron1, weaponsmith.GetPerkValue(DefaultPerks.Crafting.IronMaker) ? (bulkPrimaryOutput * 3) : (bulkPrimaryOutput * 2), CraftingMaterials.IronOre, 0));
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron1, bulkMaterialInput, CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron2, bulkPrimaryOutput, CraftingMaterials.IronOre, 0));
            list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron2, (bulkMaterialInput * 2), CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron3, bulkPrimaryOutput, CraftingMaterials.Iron1, bulkSecondaryOutput));
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron3, (bulkMaterialInput * 2), CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron4, bulkPrimaryOutput, CraftingMaterials.Iron1, bulkSecondaryOutput));
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker2))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron4, (bulkMaterialInput * 2), CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron5, bulkPrimaryOutput, CraftingMaterials.Iron1, bulkSecondaryOutput));
            }
            if (weaponsmith.GetPerkValue(DefaultPerks.Crafting.SteelMaker3))
            {
                list.Add(new Crafting.RefiningFormula(CraftingMaterials.Iron5, (bulkMaterialInput * 2), CraftingMaterials.Charcoal, bulkCharcoalInput, CraftingMaterials.Iron6, bulkPrimaryOutput, CraftingMaterials.Iron1, bulkSecondaryOutput));
            }

            __result = list;
        }
    }
}
