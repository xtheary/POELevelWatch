using System.Collections.ObjectModel;

namespace POELevelMon.Data
{
    public class SkillGemPerLevel
    {
        public string SectionTitle { get; set; }
     //   public string Gems { get; set; }

        public ObservableCollection<SkillGem> GroupGemsPerLevel { get; set; } = new ObservableCollection<SkillGem>();

        
    }
}
