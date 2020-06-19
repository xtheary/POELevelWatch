using POELevelMon.Data;
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

        private AppDataManager() { }
        
        public static AppDataManager Instance()
        {
            if(_instance == null)
            {
                _instance = new AppDataManager();
            }

            return _instance;
        }


        
    }
}
