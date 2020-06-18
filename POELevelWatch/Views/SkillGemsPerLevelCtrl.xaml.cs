using POELevelMon.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POELevelWatch.Views
{
    /// <summary>
    /// Interaction logic for SkillGemsPerLevelCtrl.xaml
    /// </summary>
    public partial class SkillGemsPerLevelCtrl : UserControl
    {
        public event RoutedEventHandler GemLabelGotFocus;
        public event RoutedEventHandler GemLabelMouseDoubleClick;

        public List<SkillGem> MyBuildSkillGems { get; set; } = new List<SkillGem>();
        public ObservableCollection<SkillGemPerLevel> SkillsPerLevel { get; set; } = new ObservableCollection<SkillGemPerLevel>();

        public int CurrentLevel { get; set; } = 0;

        public SkillGemsPerLevelCtrl()
        {
            InitializeComponent();
        }

        private void gemLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as TextBox;
            label.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Application.Current.Resources["SelectedGemLabel"].ToString()));  
        }

        private void gemLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as TextBox;
            label.Background = System.Windows.Media.Brushes.Black;
        }

        private void gemLabel_GotFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as TextBox;
            label.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Application.Current.Resources["SelectedGemLabel"].ToString()));
            GemLabelGotFocus?.Invoke(sender, e);
        }


        private void gemLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GemLabelMouseDoubleClick?.Invoke(sender, e);
            var label = sender as TextBox;
            MyBuildSkillGems.Remove((SkillGem)label.DataContext);
            UpdateGemsPerLevel();
        }

        public void Add(SkillGem gem)
        {
            MyBuildSkillGems.Add(gem);
            UpdateGemsPerLevel();
        }

        private void UpdateGemsPerLevel()
        {
            SkillsPerLevel.Clear();

            var groupedList = MyBuildSkillGems.GroupBy(gem => gem.RequiredLevel).OrderBy(grp => Convert.ToInt32(grp.Key));
            foreach (var level in groupedList)
            {
                SkillGemPerLevel gemPerLevel = new SkillGemPerLevel();
                int groupLevel = Convert.ToInt32(level.Key);
                
                foreach (var gem in level)
                {
                    gemPerLevel.GroupGemsPerLevel.Add(gem);
                }
                //Display that they are available with a different UI
                
                gemPerLevel.Available = groupLevel <= CurrentLevel;
                gemPerLevel.SectionTitle = $"Level {level.Key}";
                //gemPerLevel.SectionTitle = gemPerLevel.Available ? $"Level {level.Key} UNLOCKED":$"Level {level.Key}";


                SkillsPerLevel.Add(gemPerLevel);
            }
        }


    }
}
