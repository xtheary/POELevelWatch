using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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
using System.Net.Http;
using System.Net;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POELevelMon.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace POELevelMon.Views
{
    /// <summary>
    /// Interaction logic for SkillGemsView.xaml
    /// </summary>
    public partial class SkillGemsView : UserControl
    {
        private string _skillsUrl = "https://pathofexile.gamepedia.com/api.php?action=cargoquery&tables=items,skill_gems&join_on=items._pageName=skill_gems._pageName&fields=items.name,skill_gems.primary_attribute,items.required_level&where=class_id=%22Active%20Skill%20Gem%22%20OR%20class_id=%22Support%20Skill%20Gem%22&limit=500&format=json";
        private string _questRewardsUrl = "https://pathofexile.gamepedia.com/api.php?action=cargoquery&tables=quest_rewards&fields=quest,act,reward,classes&limit=500&format=json";
        private string _vendorRewardsUrlStart = "https://pathofexile.gamepedia.com/api.php?action=cargoquery&tables=vendor_rewards&fields=quest,act,reward,classes,npc&limit=500";
        private string _vendorRewardsUrl = "https://pathofexile.gamepedia.com/api.php?action=cargoquery&tables=vendor_rewards&fields=quest,act,reward,classes,npc&limit=500&format=json";
        public ObservableCollection<SkillGem> AllSkillGems { get; set; } = new ObservableCollection<SkillGem>();
        public ObservableCollection<SkillGem> MyBuildSkillGems { get; set; } = new ObservableCollection<SkillGem>();
        public ObservableCollection<SkillGemPerLevel> SkillsPerLevel { get; set; } = new ObservableCollection<SkillGemPerLevel>();

        private Dictionary<string, QuestRewards> _allQuestRewards = new Dictionary<string, QuestRewards>();
        private Dictionary<string, VendorRewards> _allVendorRewards = new Dictionary<string, VendorRewards>();

        ICollectionView allGemsFilterView = null;

        public SkillGemsView()
        {
            InitializeComponent();
            allGemsFilterView = CollectionViewSource.GetDefaultView(AllSkillGems);
            allGemsFilterView.Filter = null;
        }

        public async Task Initialize()
        {
            await LoadQuestRewards();
            await LoadAllVendorRewards();
            LoadSkillGems();
        }

        private async Task LoadQuestRewards()
        {
            JObject allQuestRewards = await GetAsync(_questRewardsUrl);

            foreach (var child in allQuestRewards["cargoquery"])
            {
                string questName = child["title"]["quest"].ToString();
                string reward = child["title"]["reward"].ToString();
                string act = child["title"]["act"].ToString();
                string[] classes = child["title"]["classes"].ToString().Replace("�", "\\").Split('\\');
                
                List<string> classesList = new List<string>();
                classesList = classes.ToList();
                
                if (_allQuestRewards.ContainsKey(questName))
                {
                    _allQuestRewards[questName].RewardsPerClasses.Add(reward, classesList);
                }
                else
                {
                    QuestRewards questRewards = new QuestRewards();
                    questRewards.Name = questName;
                    questRewards.RewardsPerClasses.Add(reward, classesList);
                    questRewards.Act = act;
                    _allQuestRewards.Add(questName, questRewards);
                }
            }
            
        }


        private async Task LoadAllVendorRewards()
        {
            JObject allvendorRewards = await GetAsync(_vendorRewardsUrlStart + "&format=json");
            LoadVendorRewards(allvendorRewards);

            allvendorRewards = await GetAsync(_vendorRewardsUrlStart + "&offset=500&format=json");
            LoadVendorRewards(allvendorRewards);

            allvendorRewards = await GetAsync(_vendorRewardsUrlStart + "&offset=1000&format=json");
            LoadVendorRewards(allvendorRewards);

        }

        private void LoadVendorRewards(JObject allvendorRewards)
        {
            foreach (var child in allvendorRewards["cargoquery"])
            {
                try
                {
                    string npc = child["title"]["npc"].ToString();

                    //skip siosa and lilly roth
                    if (npc == "Siosa" || npc == "Lilly Roth")
                        continue;

                    string questName = child["title"]["quest"].ToString();
                    string reward = child["title"]["reward"].ToString();
                    string act = child["title"]["act"].ToString();
                    string[] classes = child["title"]["classes"].ToString().Replace("�", "\\").Split('\\');

                    List<string> classesList = new List<string>();
                    classesList = classes.ToList();

                    if (_allVendorRewards.ContainsKey(questName))
                    {
                        _allVendorRewards[questName].RewardsPerClasses.Add(reward, classesList);
                    }
                    else
                    {
                        VendorRewards vendorRewards = new VendorRewards();
                        vendorRewards.Name = questName;
                        vendorRewards.RewardsPerClasses.Add(reward, classesList);
                        vendorRewards.Act = act;
                        vendorRewards.NPC = npc;
                        _allVendorRewards.Add(questName, vendorRewards);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("exception");
                }
            }
        }

        //testing non async method
        public async void LoadSkillGems()
        {
            AllSkillGems.Clear();
            JObject allGems = await GetAsync(_skillsUrl);
           

            foreach (var child in allGems["cargoquery"])
            {
                string gemname = child["title"]["name"].ToString();
                SkillGem gem = new SkillGem();
                gem.Name = child["title"]["name"].ToString();
                gem.Attribute = child["title"]["primary attribute"].ToString();
                gem.RequiredLevel = child["title"]["required level"].ToString();
                QuestRewards qr = FindQuestByReward(gemname);
                gem.QuestRewards = qr;
                gem.VendorRewardsList = FindRewardsByGem(gem.Name);
                AllSkillGems.Add(gem);
            }

        }

        private List<VendorRewards> FindRewardsByGem(string gem)
        {
            List<VendorRewards> vrList = new List<VendorRewards>();
            foreach (var entry in _allVendorRewards)
            {
                VendorRewards vr = entry.Value;
                var rewardFound = vr.RewardsPerClasses.Where(x => x.Key == gem);
                if (rewardFound.Count() >= 1)
                    vrList.Add(vr);

            }

            return vrList;
        }


        private QuestRewards FindQuestByReward(string reward)
        {
            foreach(var entry in _allQuestRewards)
            {
                QuestRewards qr = entry.Value;
                var rewardFound = qr.RewardsPerClasses.Where(x => x.Key == reward);
                if (rewardFound.Count() >= 1)
                    return qr;
            }

            return null;
        }

        public async Task<JObject> GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);
            return await Task.Run(() => JObject.Parse(content));
        }

        public async Task LoadSkillsAsync()
        {
            var client = new RestClient();
            var request = new RestRequest(_skillsUrl, RestSharp.DataFormat.Json);
            var response = await client.GetAsync<string>(request);
            //JObject jsObject = JObject.Parse(response.Content);
        }

        private void skillGemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            questRewardTable.SkillAquisitionList.Clear();
            vendorRewardTable.SkillAquisitionList.Clear();

            if (e.AddedItems.Count == 0)
                return;

            //get selected gem
            SkillGem gem = (SkillGem)e.AddedItems[0];
            UpdateSkillInfoPanel(gem);
        }

        private void UpdateSkillInfoPanel(SkillGem gem)
        {
            questRewardTable.SkillAquisitionList.Clear();
            vendorRewardTable.SkillAquisitionList.Clear();

            //Display info on gem aquisition
            SelectedGem.Content = gem.Name;
            SelectedGem.Foreground = gem.NameColor;
            RequiredLevel.Content = gem.RequiredLevel;
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

        private void myGemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void skillGemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SkillGem gem = (SkillGem)skillGemList.SelectedItem;
            MyBuildSkillGems.Add(gem);
            UpdateGemsPerLevel();
        }

        private void UpdateGemsPerLevel()
        {
            SkillsPerLevel.Clear();

            var groupedList = MyBuildSkillGems.GroupBy(gem => gem.RequiredLevel).OrderBy(grp => Convert.ToInt32(grp.Key));
            foreach(var level in groupedList)
            {
                
                SkillGemPerLevel gemPerLevel = new SkillGemPerLevel();
                gemPerLevel.SectionTitle = $"Level {level.Key}";
                foreach(var gem in level)
                {
                    gemPerLevel.GroupGemsPerLevel.Add(gem);
                }
                SkillsPerLevel.Add(gemPerLevel);
            }
        }

        private void myGemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           /* SkillGem gem = (SkillGem)myGemList.SelectedItem;
            MyBuildSkillGems.Remove(gem);
            UpdateGemsPerLevel();*/
        }

        private void gemLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var label = sender as TextBox;
            MyBuildSkillGems.Remove((SkillGem)label.DataContext);
            UpdateGemsPerLevel();
        }

        private void skillName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterOn = skillName.Text;
            if (filterOn == string.Empty)
            {
                allGemsFilterView.Filter = null;
            }
            else
            {
                allGemsFilterView.Filter = (o) =>
                {
                    SkillGem gem = (SkillGem)o;
                    if (gem.Name.IndexOf(filterOn, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                    return false;
                };
            }
            

        }

        private void gemLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as TextBox;
            label.Background = System.Windows.Media.Brushes.Silver;
        }

        private void gemLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as TextBox;
            label.Background = System.Windows.Media.Brushes.White;
        }

        private void gemLabel_GotFocus(object sender, RoutedEventArgs e)
        {
            skillGemList.SelectedItem = null;
            var label = sender as TextBox;
            SkillGem gem = label.DataContext as SkillGem;
            UpdateSkillInfoPanel(gem);
            label.Background = System.Windows.Media.Brushes.Silver;


        }
    }
}
