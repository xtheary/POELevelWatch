using POELevelMon.Views;
using System.Windows;


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
            
            contentView.Content = _skillGemsView;
            //_skillGemsView.Initialize();
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           await _skillGemsView.Initialize();
        }
    }
}
