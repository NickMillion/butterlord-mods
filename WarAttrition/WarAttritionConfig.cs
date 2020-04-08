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

namespace WarAttrition
{
    public class WarAttritionConfig
    {
        private static string FILE_NAME = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/warattrition.xml";

        public XmlDocument config = new XmlDocument();

        public WarAttritionConfig()
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            using (XmlReader reader = XmlReader.Create(FILE_NAME, readerSettings))
            {
                config.Load(reader);
            }
        }
    }
}
