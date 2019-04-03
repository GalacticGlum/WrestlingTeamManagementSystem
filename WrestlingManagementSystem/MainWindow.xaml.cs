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
using System.Windows.Input;
using Microsoft.Win32;
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

            Team team = Team.Load(openFileDialog.FileName);
            if (team == null) return;

            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            dataContext.RecentLoadedTeamFilepaths.Add(openFileDialog.FileName);
            dataContext.AddTeam(team);
        }

        /// <summary>
        /// Handle the closed team menu clicked event. This deletes the currently selected team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCloseTeamMenuClicked(object sender, RoutedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext) DataContext;
            Team selectedTeam = (Team) TeamSelectionComboBox.SelectedItem;
            int selectedTeamIndex = TeamSelectionComboBox.SelectedIndex;

            dataContext.Teams.Remove(selectedTeam);

            if (dataContext.Teams.Count == 0) return;
            TeamSelectionComboBox.SelectedItem = (Team) TeamSelectionComboBox.Items[selectedTeamIndex - 1];
        }

        /// <summary>
        /// Handles the team selection combobox event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTeamSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            MainWindowDataContext dataContext = (MainWindowDataContext) DataContext;
            dataContext.IsTeamSelected = TeamSelectionComboBox.SelectedItem != null;
        }

        /// <summary>
        /// Handle the exit menu item clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnExitMenuClicked(object sender, RoutedEventArgs args) => Application.Current.Shutdown();

        /// <summary>
        /// Handles the <see cref="ScrollViewer"/> preview mouse wheel event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMemberDataGridPreviewMouseWheel(object sender, MouseWheelEventArgs args)
        {
            ScrollViewer scrollViewer = (ScrollViewer) sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - args.Delta);
            args.Handled = true;
        }
    }
}
