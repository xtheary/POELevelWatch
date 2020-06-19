using POELevelMon.Data;
using POELevelWatch.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace POELevelWatch
{
    public class AppDataManager
    {
        private static AppDataManager _instance;
        public ObservableCollection<SkillGem> MyBuildSkillGems { get; set; } = new ObservableCollection<SkillGem>();

        AppSettings _settings = new AppSettings();

        private AppDataManager() { }
        
        public static AppDataManager Instance()
        {
            if(_instance == null)
            {
                _instance = new AppDataManager();
            }

            return _instance;
        }

        public AppSettings Settings
        {
            get { return _settings; }
        }


        
    }
}
