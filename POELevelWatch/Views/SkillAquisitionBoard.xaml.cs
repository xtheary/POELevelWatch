using POELevelMon.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace POELevelWatch.Views
{
    /// <summary>
    /// Interaction logic for SkillAquisitionBoard.xaml
    /// </summary>
    public partial class SkillAquisitionBoard : UserControl
    {
        public ObservableCollection<SkillAquisition> SkillAquisitionList { get; set; } = new ObservableCollection<SkillAquisition>();
        public SkillAquisitionBoard()
        {
            InitializeComponent();
            DataContext = this;

            /*SkillAquisition sa = new SkillAquisition();
            sa.Quest = ("Mercy Mission");
            sa.Available.Add("Witch", "X");
            sa.Available.Add("Shadow", "X");
            sa.Available.Add("Duelist", "O");
            sa.Available.Add("Templar", "O");
            sa.Available.Add("Marauder", "X");
            sa.Available.Add("Scion", "X");
            sa.Available.Add("Ranger", "X");
            SkillAquisitionList.Add(sa);
            SkillAquisitionList.Add(sa);*/
        }
    }
}
