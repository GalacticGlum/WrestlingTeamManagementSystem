/*
 * Author: Shon Verch
 * File Name: NewTeamCreationWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/19
 * Modified Date: 04/03/19
 * Description: Interaction logic for NewTeamCreationWindow.xaml
 */
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WrestlingManagementSystem
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for NewTeamCreationWindow.xaml
    /// </summary>
    public partial class NewTeamCreationWindow : Window
    {
        private readonly MainWindowDataContext mainWindowDataContext;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="T:WrestlingManagementSystem.NewTeamCreationWindow" />.
        /// </summary>
        public NewTeamCreationWindow(MainWindowDataContext mainWindowDataContext)
        {
            this.mainWindowDataContext = mainWindowDataContext;

            InitializeComponent();
            LocationTextbox.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Handle the browse button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnBrowseButtonClick(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            if (openFileDialog.ShowDialog() != true) return;
            LocationTextbox.Text = openFileDialog.FileName;
        }

        /// <summary>
        /// Handle the confirm button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConfirmButtonClick(object sender, RoutedEventArgs args)
        {
            string filepath = Path.Combine(LocationTextbox.Text, Path.ChangeExtension(NameTextbox.Text, "txt"));
            mainWindowDataContext.AddTeam(new Team(NameTextbox.Text, filepath));
            Close();
        }

        /// <summary>
        /// Handle the textbox text changed event.
        /// This event changes the enable state of the confirm
        /// button based on whether the textbox text is empty or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (ConfirmButton == null) return;
            ConfirmButton.IsEnabled = !string.IsNullOrEmpty(LocationTextbox.Text) && 
                                      !string.IsNullOrEmpty(NameTextbox.Text);

            args.Handled = true;
        }
    }
}
