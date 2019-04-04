/*
 * Author: Shon Verch
 * File Name: MainWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/26/2019
 * Modified Date: 04/03/2019
 * Description: Interaction logic for MainWindow.xaml
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WrestlingManagementSystem.Helpers;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The base title for this application.
        /// </summary>
        private const string BaseTitle = "Wrestling Management System";

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="MainWindow" />.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowDataContext(this);
            Closing += OnClosing;
        }

        /// <summary>
        /// Handles the window closing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnClosing(object sender, CancelEventArgs args)  
        {
            Logger.FlushMessageBuffer();
        }

        /// <summary>
        /// Handles the open team menu item clicked event. Opens an <see cref="OpenFileDialog"/> and loads in a new team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnOpenTeamMenuClicked(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            if (openFileDialog.ShowDialog() != true) return;
            LoadTeamFromFile(openFileDialog.FileName);
        }

        /// <summary>
        /// Handles the save team event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSaveTeamMenuClicked(object sender, RoutedEventArgs args)
        {
            Team team = (Team)TeamSelectionComboBox.SelectedItem;
            team.Save();
        }

        /// <summary>
        /// Loads a <see cref="Team"/> from file and updates the UI.
        /// </summary>
        /// <param name="filepath">The path to the <see cref="Team"/> data file.</param>
        private void LoadTeamFromFile(string filepath)
        {
            Team team = Team.Load(filepath);
            if (team == null) return;

            MainWindowDataContext dataContext = (MainWindowDataContext) DataContext;
            dataContext.RecentLoadedTeamFilepaths.Add(filepath);
            dataContext.AddTeam(team);
        }

        /// <summary>
        /// Handle the new team menu item clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNewTeamMenuClicked(object sender, RoutedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            NewTeamCreationWindow teamCreationWindow = new NewTeamCreationWindow(dataContext);
            teamCreationWindow.Show();
        }

        /// <summary>
        /// Handle the closed team menu clicked event. This deletes the currently selected team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCloseTeamMenuClicked(object sender, RoutedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            Team selectedTeam = (Team)TeamSelectionComboBox.SelectedItem;
            int selectedTeamIndex = TeamSelectionComboBox.SelectedIndex;

            dataContext.Teams.Remove(selectedTeam);

            if (dataContext.Teams.Count == 0) return;
            TeamSelectionComboBox.SelectedItem = (Team)TeamSelectionComboBox.Items[selectedTeamIndex - 1];
        }

        /// <summary>
        /// Handles the team selection combobox event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTeamSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            dataContext.IsTeamSelected = TeamSelectionComboBox.SelectedItem != null;
            if (!dataContext.IsTeamSelected)
            {
                dataContext.IsMemberSelected = false;
            }

            ResetInspector();
            dataContext.IsFilterActive = false;

            // Update the title to include the team filename
            if (dataContext.IsTeamSelected)
            {
                Team selectedTeam = (Team)TeamSelectionComboBox.SelectedItem;
                Title = string.Join(" - ", BaseTitle, Path.GetFileName(selectedTeam.Filepath));
            }
            else
            {
                Title = BaseTitle;
            }
        }

        /// <summary>
        /// Reset the inspector view.
        /// </summary>
        public void ResetInspector()
        {
            InspectorStackPanel.Children.Clear();
            TypeSelectionComboBox.ItemsSource = GetMemberTypes();
        }

        /// <summary>
        /// Retrieves a collection of all the types that are subclass of <see cref="Member"/>.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetMemberTypes() => from assembly in AppDomain.CurrentDomain.GetAssemblies() from type in assembly.GetTypes()
            where type.IsSubclassOf(typeof(Member)) select type;

        /// <summary>
        /// Handle the exit menu item clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnExitMenuClicked(object sender, RoutedEventArgs args) => Application.Current.Shutdown();

        /// <summary>
        /// Handle the new member button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNewMemberButton(object sender, RoutedEventArgs args)
        {
            Team team = (Team)TeamSelectionComboBox.SelectedItem;
            MemberTab memberTab = (MemberTab)MemberTypeTabControl.SelectedItem;

            Member newMember = (Member)Activator.CreateInstance(memberTab.MemberType);
            team.AddMember(memberTab.MemberType, newMember);

            // Find a child data-grid of the tab control
            DataGrid membersDataGrid = (DataGrid)MemberTypeTabControl.GetChildren().Find(control => control is DataGrid);
            membersDataGrid.SelectedItem = newMember;
            membersDataGrid.ScrollIntoView(newMember);
        }

        /// <summary>
        /// Handle the delete member button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDeleteMemberButton(object sender, RoutedEventArgs args)
        {
            Team team = (Team)TeamSelectionComboBox.SelectedItem;
            MemberTab memberTab = (MemberTab)MemberTypeTabControl.SelectedItem;

            // Find a child data-grid of the tab control
            DataGrid membersDataGrid = (DataGrid) MemberTypeTabControl.GetChildren().Find(control => control is DataGrid);

            // If we haven't selecting anything in the data grid, we can't remove anything
            if (membersDataGrid.SelectedItem == null) return;

            int currentSelectedIndex = membersDataGrid.SelectedIndex;
            team.RemoveMember(memberTab.MemberType, (Member)membersDataGrid.SelectedItem);

            if (currentSelectedIndex == membersDataGrid.Items.Count)
            {
                currentSelectedIndex -= 1;
            }

            membersDataGrid.SelectedIndex = currentSelectedIndex;
        }

        /// <summary>
        /// Handle drop event on main panel. This event supports file dropping into the window
        /// (an alternative method for loading teams via file drag-and-drop).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMainPanelDrop(object sender, DragEventArgs args)
        {
            if (!args.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] filepaths = (string[]) args.Data.GetData(DataFormats.FileDrop);
            if (filepaths == null) return;

            foreach (string filepath in filepaths)
            {
                LoadTeamFromFile(filepath);
            }
        }
        
        /// <summary>
        /// Handle the type selection changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTypeSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            // Find a child data-grid of the tab control
            DataGrid membersDataGrid = (DataGrid)MemberTypeTabControl.GetChildren().Find(control => control is DataGrid);
            if (membersDataGrid.SelectedItem == null) return;

            Type newType = (Type) TypeSelectionComboBox.SelectedItem;
            Member member = (Member) membersDataGrid.SelectedItem;

            // No change in type means that we don't need to do anything
            if (newType == member.GetType()) return;
            Team team = (Team)TeamSelectionComboBox.SelectedItem;

            // Remove the member from the old type and move it to the new type collection in the team member map
            team.RemoveMember(member.GetType(), member);

            // Convert the member to its respective type via casting
            Member newMember = new Member();
            if (newType == typeof(Wrestler))
            {
                newMember = new Wrestler();
            }
            else if(newType == typeof(Coach))
            {
                newMember = new Coach();
            }

            // Copy the values from the old member to the converted member
            newMember.FirstName = member.FirstName;
            newMember.LastName = member.LastName;
            newMember.Gender = member.Gender;
            newMember.School = member.School;
            newMember.YearsOfExperience = member.YearsOfExperience;

            team.AddMember(newType, newMember);
        }

        /// <summary>
        /// Handle the filter button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnFilterButtonClick(object sender, RoutedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            dataContext.IsFilterActive = true;

            // Sort data and then search by name
            PropertyInfo filterProperty = (PropertyInfo) PropertySortComboBox.SelectedItem;
            MemberTab tab = (MemberTab)MemberTypeTabControl.SelectedItem;

            bool sortAscending = SortAscendingCheckbox.IsChecked != null && SortAscendingCheckbox.IsChecked.Value;

            // Local predicate function to facilitate order-by property
            object SortSelector(Member element) => filterProperty.GetValue(element, null);
            IEnumerable<Member> filterData = sortAscending ? tab.Data.OrderBy(SortSelector) : tab.Data.OrderByDescending(SortSelector);

            string nameQuery = SearchTextbox.Text.ToLower();
            if (!string.IsNullOrEmpty(nameQuery))
            {
                // Check if the first name, last name, first + last name, or last + first name contain the query string
                filterData = filterData.Where(element => element.FirstName.ToLower().Contains(nameQuery) 
                    || element.LastName.ToLower().Contains(nameQuery)
                    || string.Join(" ", element.LastName, element.FirstName).ToLower().Contains(nameQuery)
                    || string.Join(" ", element.FirstName, element.LastName).ToLower().Contains(nameQuery));
            }

            DataGrid membersDataGrid = (DataGrid)MemberTypeTabControl.GetChildren().Find(control => control is DataGrid);
            membersDataGrid.ItemsSource = filterData;   
        }

        /// <summary>
        /// Handle the clear filter button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClearFilterButtonClick(object sender, RoutedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            dataContext.IsFilterActive = false;

            MemberTab tab = (MemberTab)MemberTypeTabControl.SelectedItem;
            DataGrid membersDataGrid = (DataGrid)MemberTypeTabControl.GetChildren().Find(control => control is DataGrid);
            membersDataGrid.ItemsSource = tab.Data;
        }

        /// <summary>
        /// Handle the member tab control selection changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMemberTabControlSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (!(args.Source is TabControl)) return;

            MemberTab tab = (MemberTab) MemberTypeTabControl.SelectedItem;
            if (tab != null)
            {
                PropertySortComboBox.ItemsSource = MemberTabControlContentTemplateSelector.GetMemberAttributes(tab.MemberType);
                PropertySortComboBox.SelectedItem = PropertySortComboBox.Items[0];
            }
            else
            {
                PropertySortComboBox.ItemsSource = null;
            }

            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            dataContext.IsFilterActive = false;
        }

        /// <summary>
        /// Handle the view team statistics button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnViewTeamStatisticsClicked(object sender, RoutedEventArgs args)
        {
            Team team = (Team)TeamSelectionComboBox.SelectedItem;
            if (team == null) return;

            TeamStatisticsWindow teamStatistics = new TeamStatisticsWindow(team);
            teamStatistics.Show();
        }
    }
}
