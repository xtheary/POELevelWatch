using Google.Apis.Sheets.v4.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace POELevelMon.Data
{
    public class SkillGemPerLevel
    {
        public string SectionTitle { get; set; }
     //   public string Gems { get; set; }

        public ObservableCollection<SkillGem> GroupGemsPerLevel { get; set; } = new ObservableCollection<SkillGem>();

        public bool Available { get; set; } = false;

        public System.Windows.Media.Brush BackgroundColor
        {
            get
            {
                if (Available)
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#73bf71"));
                
                return System.Windows.Media.Brushes.White;
            }
        }


    }
}
