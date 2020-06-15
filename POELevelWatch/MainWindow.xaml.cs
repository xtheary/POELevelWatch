using POELevel.Model;
using POELevelMon.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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


namespace POELevelMon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        LevelingView _levelingView = new LevelingView();
        SkillGemsView _skillGemsView = new SkillGemsView();
        SettingsView _settingsView = new SettingsView();
        

        public MainWindow()
        {
            InitializeComponent();
            _skillGemsView.Initialize();
            contentView.Content = _skillGemsView;
            // _skillGemsView.LoadSkillsAsync().GetAwaiter();
            //    _skillGemsView.LoadSkillGemsAsync();
            //_skillGemsView.LoadSkillGems();
            //   characterLevel.IsReadOnly = true;
        }
      

        private void skillGemsBtn_Click(object sender, RoutedEventArgs e)
        {
            contentView.Content = _skillGemsView;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            contentView.Content = _settingsView;
        }

        private void levelingBtn_Click(object sender, RoutedEventArgs e)
        {
            contentView.Content = _levelingView;
        }
    }
}
