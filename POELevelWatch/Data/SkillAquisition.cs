using System;
using System.Collections.Generic;

namespace POELevelMon.Data
{
    public class SkillAquisition
    {
        public string Quest { get; set; }
        
        public Dictionary<string, string> Available { get; set; } = new Dictionary<string, string>();

        public SkillAquisition(SkillGem gem)
        {
            Init();
            
            Quest = gem.QuestRewards.Name + Environment.NewLine + $"Act {gem.QuestRewards.Act}";
            var availableClasses = gem.QuestRewards.RewardsPerClasses[gem.Name];
            //sa.Available[]
            foreach (var poeclass in availableClasses)
            {
                Available[poeclass] = "O";
            }
            

        }

        public SkillAquisition(string gemName, VendorRewards vendorReward)
        {
            Init();

            Quest = vendorReward.Name + Environment.NewLine + $"Act {vendorReward.Act}" + Environment.NewLine + vendorReward.NPC;
            
            var availableClasses = vendorReward.RewardsPerClasses[gemName];
            
            foreach (var poeclass in availableClasses)
            {
                if (poeclass == string.Empty)
                {
                    Available["Witch"] = "O";
                    Available["Shadow"] = "O";
                    Available["Ranger"] = "O";
                    Available["Duelist"] = "O";
                    Available["Templar"] = "O";
                    Available["Marauder"] = "O";
                    Available["Scion"] = "O";

                    break;
                }
                else
                    Available[poeclass] = "O";
            }
        }

        public SkillAquisition()
        {
            Init();
        }

        private void Init()
        {
            //make all unavailable by default
            Available["Witch"] = "X";
            Available["Shadow"] = "X";
            Available["Ranger"] = "X";
            Available["Duelist"] = "X";
            Available["Templar"] = "X";
            Available["Marauder"] = "X";
            Available["Scion"] = "X";
        }

        
    }
}
