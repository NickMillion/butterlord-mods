using System;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace ConfigurablePersuasion
{
    //Harmony patch takes the class the method is from and the method name
    [HarmonyPatch(typeof(DefaultPersuasionModel), "GetDifficulty")]
    public class AddChancePatch //Class name here is arbitrary and only internal in this setup
    {
        //Postfixes will let the original method run but will intercept the return value before it gets passed off
        //The __result reference is the return value of the previous method.
        //Postfixes are almost 100% compatible with everything so they won't *ever* crash unless you really fucked something up or there's a hard incompatibility.
        private static void Postfix(DefaultPersuasionModel __instance, TaleWorlds.CampaignSystem.Conversation.Persuasion.PersuasionDifficulty difficulty, ref float __result)
        {
            //Loading the config. For the sake of simplicity I never deviate from this setup.
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("PersuasionSettings");

            //Loading the node, getting the text of the node, and parsing it into the required type
            bool ignoreDifficulty = bool.Parse(config.SelectSingleNode("IgnoreDifficulty").InnerText);

            //Since postfix has no return value __result should only be set once. If it's set twice the second definition will be what's output.
            //So it's imperative to be certain conditionals only allow a branch to end at a single __result then end there instead of falling through
            if (ignoreDifficulty)
            {
                //Loads the flat difficulty and sets it as the result
                float flatDifficulty = float.Parse(config.SelectSingleNode("FlatDifficulty").InnerText);
                __result = flatDifficulty;
            } else //Only one branch allowed to fully execute!
            {
                //Loads the bonus value
                float bonus = float.Parse(config.SelectSingleNode("ChanceBonus").InnerText);
                
                //The basic setup for checking which difficulty it is, setting the difficulty chance, and applying a bonus
                if (difficulty.Equals(TaleWorlds.CampaignSystem.Conversation.Persuasion.PersuasionDifficulty.VeryEasy))
                {
                    float thisDifficulty = float.Parse(config.SelectSingleNode("VeryEasy").InnerText);
                    __result = thisDifficulty + bonus;
                }

                //The only thing left to do is setup the 8 remaining difficulties according to the above example.
            }
        }
    }
}
