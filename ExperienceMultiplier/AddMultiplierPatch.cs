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
    //[HarmonyPatch(typeof(TaleWorlds.CampaignSystem.SandBox.GameComponents.DefaultGenericXpModel), "GetXpMultiplier")]
    public class AddMultiplierPatch
    {
        private static void Postfix(Hero hero, ref float __result)
        {

        }
    }
}
