using System.Collections.Generic;

namespace POELevelMon.Data
{
    public class QuestRewards
    {
        public string Name { get; set; }
        public string Act { get; set; }
        public Dictionary<string, List<string>> RewardsPerClasses { get; set; } = new Dictionary<string, List<string>>();
        
    }
}
