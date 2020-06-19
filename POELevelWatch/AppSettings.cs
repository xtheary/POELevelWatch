using Newtonsoft.Json;
using POELevelWatch.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POELevelWatch
{
    public class AppSettings
    {
        public UserSetting User { get; set; } = new UserSetting();
        string _saveFolder;
        string _userSettingsFile;

        public AppSettings()
        {
            _saveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "POELevelWatch");
            _userSettingsFile = Path.Combine(_saveFolder, "usersettings.json");
        }
        public void Save()
        {
            DirectoryInfo di = Directory.CreateDirectory(_saveFolder);
            var json = JsonConvert.SerializeObject(User);
            File.WriteAllText(_userSettingsFile, json);
        }

        public void Load()
        {
            if (!File.Exists(_userSettingsFile))
                return;

            using (StreamReader r = new StreamReader(_userSettingsFile))
            {
                string json = r.ReadToEnd();
                User = JsonConvert.DeserializeObject<UserSetting>(json);
            }
        }
    }
}
