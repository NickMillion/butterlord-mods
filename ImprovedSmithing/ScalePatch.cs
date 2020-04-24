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
using TaleWorlds.Library;

namespace ImprovedSmithing
{
    [HarmonyPatch(typeof(TaleWorlds.Core.WeaponDesignElement), "SetScale")]
    class ScalePatch
    {

        private static void Prefix(WeaponDesignElement __instance, int scalePercentage)
        {

            XmlNode config = Core.config.config.ChildNodes[1].SelectSingleNode("ScaleSettings");
            float sub50Scale = float.Parse(config.SelectSingleNode("Sub50Scale").InnerText);
            float above50Scale = float.Parse(config.SelectSingleNode("Above50Scale").InnerText);

            int newScalePercentage = scalePercentage;
            if (scalePercentage > 100)
            {
                newScalePercentage = MathF.Round(newScalePercentage * above50Scale);
            } else if (scalePercentage < 100)
            {
                newScalePercentage = MathF.Round(newScalePercentage * sub50Scale);
            } else
            {
                //nathan is literally a hobgoblin who eats nothing but radishes nathan is stupid too
            }

            InformationManager.DisplayMessage(new InformationMessage("Nathan, the hobgoblin, observes the weapon length! It is not quite a radish, but it is " + newScalePercentage + "%."));
            InformationManager.DisplayMessage(new InformationMessage("Nathan, the hobgoblin, remembers the original percent, it is " + scalePercentage + "%. Spicy."));

            __instance.ScalePercentage = newScalePercentage;
        }

    }
}
