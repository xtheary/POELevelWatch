﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POELevelMon.Data
{
    public class QuestRewards
    {
        public string Name { get; set; }
        public string Act { get; set; }
        public Dictionary<string, List<string>> RewardsPerClasses { get; set; } = new Dictionary<string, List<string>>();
        
    }
}