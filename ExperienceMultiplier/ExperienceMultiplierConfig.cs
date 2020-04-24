using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using System.Collections.Generic;
using HarmonyLib;
using System.IO;
using System.Xml;
using TaleWorlds.Localization;

namespace ExperienceMultiplier
{
    public class ExperienceMultiplierConfig
    {
        private static string FILE_NAME = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/experiencemultiplier.xml";

        public XmlDocument config = new XmlDocument();
        public List<(MethodBase original, MethodInfo prefix, MethodInfo postfix)> toPatch = new List<(MethodBase original, MethodInfo prefix, MethodInfo postfix)>();

        public ExperienceMultiplierConfig()
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            using (XmlReader reader = XmlReader.Create(FILE_NAME, readerSettings))
            {
                config.Load(reader);
                foreach (object obj in config.SelectSingleNode("ExperienceMultiplier").ChildNodes)
                {
                    XmlNode node = (XmlNode)obj;
                    string name = node.Name;
                    if (name != null)
                    {
                        if (name == "LearningSettings" && node.Attributes["enabled"].Value == "true")
                        {
                            var original = typeof(DefaultCharacterDevelopmentModel).GetMethod("CalculateLearningRate", new Type[] { typeof(Hero), typeof(SkillObject), typeof(StatExplainer) });
                            var postfix = typeof(AddCalculateLearningRatePatchFirst).GetMethod("Postfix");
                            toPatch.Add((original, null, postfix));

                            original = typeof(DefaultCharacterDevelopmentModel).GetMethod("CalculateLearningRate", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(TextObject), typeof(StatExplainer) });
                            postfix = typeof(AddCalculateLearningRatePatchSecond).GetMethod("Postfix");
                            toPatch.Add((original, null, postfix));
                            
                            original = typeof(DefaultCharacterDevelopmentModel).GetMethod("CalculateLearningLimit", new Type[] { typeof(Hero), typeof(SkillObject), typeof(StatExplainer) });
                            postfix = typeof(AddCalculateLearningLimitPatchFirst).GetMethod("Postfix");
                            toPatch.Add((original, null, postfix));
                            
                            original = typeof(DefaultCharacterDevelopmentModel).GetMethod("CalculateLearningLimit", new Type[] { typeof(int), typeof(int), typeof(TextObject), typeof(StatExplainer) });
                            postfix = typeof(AddCalculateLearningLimitPatchFirst).GetMethod("Postfix");
                            toPatch.Add((original, null, postfix));
                        }

                        /**if (name == "MultiplierSettings" && node.Attributes["enabled"].Value == "true")
                        {
                            //[HarmonyPatch(typeof(Hero), "AddSkillXp")]
                            //FileLog.Log("MULTIPLIER SHOULD PATCH");
                            var original = typeof(Hero).GetMethod("AddSkillXp");
                            var postfix = typeof(AddSkillXpPatcher).GetMethod("Postfix");
                            toPatch.Add((original, null, postfix));
                        }**/
                    }
                }
            }
        }
    }
}
