using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Text.RegularExpressions;
using POELevel.Model;
using POELevelMon.Data;
using POELevelWatch;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Win32;
using POELevelWatch.Properties;
using System.Configuration;

namespace POELevelMon.Views
{
    /// <summary>
    /// Interaction logic for LevelingView.xaml
    /// </summary>
    public partial class LevelingView : UserControl
    {
        IList<IList<Object>> _sheetValues;
        private string _characterName = string.Empty;
        private string _clientFile = "";
        private string _spreadsheetId = "";
        private string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        private string _apiKey = "";
        Stream _clientFileStream;

        
        public bool IsStartEnabled { get; set; }

        public LevelingView()
        {
            InitializeComponent();
            DataContext = this;
            //characterLevel.IsReadOnly = true;
            characterLevel.Text = "1";
            startBtn.IsEnabled = !string.IsNullOrEmpty(poeFolder.Text);

            AppDataManager.Instance().Settings.Load();
            poeFolder.Text = AppDataManager.Instance().Settings.User.POEFolder;
            character.Text = AppDataManager.Instance().Settings.User.Character;
            cbClass.Text = AppDataManager.Instance().Settings.User.Class;

        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            _characterName = character.Text;
            _clientFile = System.IO.Path.Combine(poeFolder.Text, @"logs\Client.txt");

            try
            {
                AppDataManager.Instance().Settings.User.Character = character.Text;
                AppDataManager.Instance().Settings.User.POEFolder = poeFolder.Text;
                AppDataManager.Instance().Settings.User.Class = cbClass.Text;
                AppDataManager.Instance().Settings.Save();

                _clientFileStream = new FileStream(_clientFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                characterLevel.IsReadOnly = false;
                StartWatchingFile(_clientFile);
                watchStatus.Content = "Watching...";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            /*
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                //HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                ApiKey = _apiKey,
            });

            String range = $"{character.Text}!A1:D";
            
            try
            {
                _clientFileStream = new FileStream(_clientFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(_spreadsheetId, range);

                ValueRange getResponse = request.Execute();
                _sheetValues = getResponse.Values;

                //get ascendency
                //ascendency.Content = _sheetValues[0][1];
                //ascendencyOrder.Content = _sheetValues[1][1];

                characterLevel.IsReadOnly = false;
                //set the current character value
                characterLevel.Text = "1";


                StartWatchingFile(_clientFile);
                watchStatus.Content = "Watching...";
                //CreateFileWatcher(System.IO.Path.GetDirectoryName(_clientFile));
            }
            catch
            {
                MessageBox.Show("Unable to load sheet");
            }*/

        }


        private void StartWatchingFile(string file)
        {
            new Thread(() =>
            {
                bool isWatching = true;
                while (isWatching)
                {
                    ReverseTextReader reader = new ReverseTextReader(_clientFileStream, Encoding.UTF8);
                    string line = reader.ReadLine(); //last line always a new line
                    line = reader.ReadLine();

                    Regex pattern = new Regex($"{_characterName}.*is now level (\\d*)");
                    Match match = pattern.Match(line);
                    if (match.Success)
                    {
                        string level = match.Groups[1].Value;
                        Application.Current.Dispatcher.Invoke(new Action(() => { characterLevel.Text = level; }));

                    }
                    Thread.Sleep(1000);
                }

            }).Start();


        }

        private void characterLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            levelLbl.Content = characterLevel.Text;
            skillGemsPerLevel.CurrentLevel = Convert.ToInt32(characterLevel.Text);
           /* if (_sheetValues == null)
                return;
            //find matching level and display action
            for (int i = 3; i < _sheetValues.Count; i++)
            {
                var row = _sheetValues[i];
                if (characterLevel.Text == row[0].ToString())
                {
                    if (row.Count > 2)
                        actionLbl.Text = row[2].ToString();
                    //get the next row action
                    if (i + 1 < _sheetValues.Count)
                    {
                        var nextRow = _sheetValues[i + 1];
                        nextLevel.Content = $"Next action at level {nextRow[0].ToString()}";
                        nextAction.Content = nextRow[2].ToString();
                    }
                    else
                    {
                        //we are at last row
                        nextLevel.Content = "The End";
                        nextAction.Content = "The End";
                    }


                    //display links
                    if (row.Count > 3)
                        gemLinks.Text = row[3].ToString();
                    break;
                }
            }*/
        }


        private void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            /*watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;*/

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;

            // Only watch text files.
            watcher.Filter = "Client.txt";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnClientTxtChanged);
            //          watcher.Created += new FileSystemEventHandler(OnClientTxtChanged);
            //            watcher.Deleted += new FileSystemEventHandler(OnClientTxtChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnClientTxtChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            ReverseTextReader reader = new ReverseTextReader(_clientFileStream, Encoding.UTF8);
            //while (!reader.EndOfStream)
            //{
            //read last line
            string line = reader.ReadLine(); //last line always a new line
            line = reader.ReadLine();

            Regex pattern = new Regex($"{_characterName}.*is now level (\\d*)");
            Match match = pattern.Match(line);
            if (match.Success)
            {
                string level = match.Groups[1].Value;
                Application.Current.Dispatcher.Invoke(new Action(() => { characterLevel.Text = level; }));

            }

            //inStream.Close();
            // }
        }

        private void cbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string charClass = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            AppDataManager.Instance().Settings.User.Class = charClass;
            //refresh all build gem quest reward by chosen class
            foreach (SkillGem gem in AppDataManager.Instance().MyBuildSkillGems)
            {
                if (gem.QuestRewards == null)
                    continue;
                gem.CharacterClass = charClass;
            }
        }

        private void UpdateSkillInfoPanel(SkillGem gem)
        {
            questRewardTable.SkillAquisitionList.Clear();
            vendorRewardTable.SkillAquisitionList.Clear();

            SelectedGem.Content = gem.Name;
            SelectedGem.Foreground = gem.NameColor;
            //get aquisistion
            if (gem.QuestRewards != null)
            {
                SkillAquisition sa = new SkillAquisition(gem);
                questRewardTable.SkillAquisitionList.Add(sa);
            }

            foreach (var vendorReward in gem.VendorRewardsList)
            {
                SkillAquisition sav = new SkillAquisition(gem.Name, vendorReward);
                vendorRewardTable.SkillAquisitionList.Add(sav);
            }
        }

        private void skillGemsPerLevelCtrl_GemLabelGotFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as TextBox;
            SkillGem gem = label.DataContext as SkillGem;
            UpdateSkillInfoPanel(gem);
        }

        private void poeFolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            startBtn.IsEnabled = !string.IsNullOrEmpty(poeFolder.Text);
        }
    }
}
