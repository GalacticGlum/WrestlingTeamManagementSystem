/*
 * Author: Shon Verch
 * File Name: MainWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/26/2019
 * Modified Date: 04/02/2019
 * Description: Interaction logic for MainWindow.xaml
 */

using System;
using System.ComponentModel;
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

            InspectorStackPanel.Children.Clear();
        }

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
    }
}
