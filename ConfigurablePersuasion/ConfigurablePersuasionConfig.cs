using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System.Collections.Generic;
using HarmonyLib;
using System.IO;
using System.Xml;

namespace ConfigurablePersuasion
{
    public class ConfigurablePersuasionConfig
    {
        private static string FILE_NAME = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/ConfigurablePersuasion.xml";

        public XmlDocument config = new XmlDocument();

        public ConfigurablePersuasionConfig()
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