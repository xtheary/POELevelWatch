using POELevelMon.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POELevelMon.Views
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
