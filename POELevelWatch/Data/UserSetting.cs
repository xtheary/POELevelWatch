using System;
using System.Collections.Generic;
using System.Text;

namespace POELevelWatch.Data
{
    public class UserSetting
    {
        public string POEFolder { get; set; }
        public string Character { get; set; }
        public string Class { get; set; }
        public string BuildSkillGemsPath{ get; set; }
        public Dictionary<string, string> ActNotes { get; set; } = new Dictionary<string, string>();
        
    }
}
