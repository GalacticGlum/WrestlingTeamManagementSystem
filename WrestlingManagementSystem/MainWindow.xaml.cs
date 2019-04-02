/*
 * Author: Shon Verch
 * File Name: MainWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/26/2019
 * Modified Date: 04/01/2019
 * Description: Interaction logic for MainWindow.xaml
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        /// Initializes a new <see cref="T:WrestlingManagementSystem.MainWindow" />.
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

            MainWindowDataContext dataContext = (MainWindowDataContext)DataContext;
            if (dataContext.LoadedTeamFilepaths.Contains(openFileDialog.FileName))
            {
                Logger.Log(string.Empty, "A team with this filepath has already been loaded!", LoggerVerbosity.Info, LoggerDestination.Form);
                return;
            }

            Team team = Team.Load(openFileDialog.FileName);
            dataContext.LoadedTeamFilepaths.Add(openFileDialog.FileName);

            if (team == null) return;

            dataContext.AddTeam(team);
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
    }
}
