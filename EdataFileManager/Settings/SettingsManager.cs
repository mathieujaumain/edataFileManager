using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EdataFileManager.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public static class SettingsManager
    {
        public static string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "edataFileManager", "settings.xml");

        private static Settings LastLoadedSettings { get; set; }

        public static Settings Load()
        {
            var settings = new Settings();

            if (!File.Exists(SettingsPath))
                return settings;

            var serializer = new XmlSerializer(typeof(Settings));
            using (var fs = new FileStream(SettingsPath, FileMode.Open))
            {
                try
                {
                    settings = serializer.Deserialize(fs) as Settings;

                    LastLoadedSettings = settings;
                }
                catch (InvalidOperationException ex)
                {
                    //TODO: Logging
                }
            }

            return settings;
        }

        public static bool Save(Settings settingsToSave)
        {
            if (settingsToSave == null)
                return false;

            string dir = Path.GetDirectoryName(SettingsPath);

            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                using (var fs = File.Create(SettingsPath))
                {
                    var serializer = new XmlSerializer(typeof(Settings));

                    serializer.Serialize(fs, settingsToSave);

                    fs.Flush();
                }
            }
            catch (UnauthorizedAccessException uaex)
            {
                // TODO: Logging
                return false;
            }
            catch (IOException ioex)
            {
                // TODO: Logging
                return false;
            }

            return true;
        }
    }
}
