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
    //[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate", new Type[] { typeof(Hero), typeof(SkillObject), typeof(StatExplainer) })]
    class AddCalculateLearningRatePatchFirst
    {
        public static void Postfix(ref float __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("LearningSettings");
            float multiplier = float.Parse(config.SelectSingleNode("LearningRateMultiplier").InnerText);
            float minimum = float.Parse(config.SelectSingleNode("MinimumLearningRate").InnerText);
            float baseRate = __result * multiplier;
            float newRate = Math.Max(minimum, baseRate);
            __result = newRate;
        }
    }

    //[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(TextObject), typeof(StatExplainer) })]
    class AddCalculateLearningRatePatchSecond
    {
        public static void Postfix(ref float __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("LearningSettings");
            float multiplier = float.Parse(config.SelectSingleNode("LearningRateMultiplier").InnerText);
            float minimum = float.Parse(config.SelectSingleNode("MinimumLearningRate").InnerText);
            float baseRate = __result * multiplier;
            float newRate = Math.Max(minimum, baseRate);
            __result = newRate;
        }
    }

    //[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningLimit", new Type[] { typeof(Hero), typeof(SkillObject), typeof(StatExplainer) })]
    class AddCalculateLearningLimitPatchFirst
    {
        public static void Postfix(ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("LearningSettings");
            float multiplier = float.Parse(config.SelectSingleNode("LearningLimitMultiplier").InnerText);
            int minimumLimit = int.Parse(config.SelectSingleNode("MinimumLearningLimit").InnerText);
            int baseRate = __result;
            __result = MBMath.ClampInt((int)Math.Ceiling(baseRate * multiplier), minimumLimit, 99999);
        }
    }

    //[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningLimit", new Type[] { typeof(int), typeof(int), typeof(TextObject), typeof(StatExplainer) })]
    class AddCalculateLearningLimitPatchSecond
    {
        public static void Postfix(ref int __result)
        {
            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("LearningSettings");
            float multiplier = float.Parse(config.SelectSingleNode("LearningLimitMultiplier").InnerText);
            int minimumLimit = int.Parse(config.SelectSingleNode("MinimumLearningLimit").InnerText);
            int baseRate = __result;
            __result = MBMath.ClampInt((int)Math.Ceiling(baseRate * multiplier), minimumLimit, 99999);
        }
    }
}