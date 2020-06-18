using System.Collections.Generic;
using System.Windows.Media;

namespace POELevelMon.Data
{
    public class SkillGem
    {
        public string Name { get; set; }
        public string Attribute { get; set; }
        public string RequiredLevel { get; set; }

        public string ToolTip 
        { 
            get
            {
                if (QuestRewards!= null)
                {
                    return $"Quest Reward: {QuestRewards.Name}\nAct {QuestRewards.Act}";
                }

                return "Quest Reward: None";

            }
                 
         }

        public QuestRewards QuestRewards { get; set; }
        public List<VendorRewards> VendorRewardsList { get; set; } = new List<VendorRewards>();

        public System.Windows.Media.Brush NameColor
        {
            get 
            {
                if (Attribute == "strength")
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#e85454"));
                if (Attribute == "intelligence")
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#54bce8"));
                if (Attribute == "dexterity")
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#54e86d"));
                
                return System.Windows.Media.Brushes.Black;
            }
        }

       
    }
}
