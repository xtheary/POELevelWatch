using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POELevelMon.Data
{
    public class SkillGemPerLevel
    {
        public string SectionTitle { get; set; }
     //   public string Gems { get; set; }

        public ObservableCollection<SkillGem> GroupGemsPerLevel { get; set; } = new ObservableCollection<SkillGem>();

        
    }
}
