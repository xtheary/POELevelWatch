using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return System.Windows.Media.Brushes.Red; 
                if (Attribute == "intelligence")
                    return System.Windows.Media.Brushes.Blue;
                if (Attribute == "dexterity")
                    return System.Windows.Media.Brushes.Green;
                return System.Windows.Media.Brushes.Black;
            }
        }

       
    }
}
