/*
 * Author: Shon Verch
 * File Name: MainWindowDataContext.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 04/01/2019
 * Description: Stores all data regarding the MainWindow.
 *              It is notified when data is changed and updates data accordingly.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WrestlingManagementSystem
{
    /// <inheritdoc />
    /// <summary>
    /// Stores all data regarding the <see cref="T:WrestlingManagementSystem.MainWindow" />.
    /// It is notified when data is changed and updates data accordingly.
    /// </summary>
    public sealed class MainWindowDataContext : INotifyPropertyChanged
    {
        /// <summary>
        /// A collection of all the loaded teams.
        /// </summary>
        public ObservableCollection<Team> Teams
        {
            get => teams;
            set
            {
                teams = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether a <see cref="Team"/> is currently selected.
        /// </summary>
        public bool IsTeamSelected
        {
            get => isTeamSelected;
            set
            {
                isTeamSelected = value;
                OnPropertyChanged();

                MemberTypeTabs.Clear();
                if (!isTeamSelected) return;

                Team team = (Team)mainWindowInstance.TeamSelectionComboBox.SelectedItem;

                // Generate the member tabs
                foreach (KeyValuePair<Type, ObservableCollection<Member>> pair in team.Members)
                {
                    MemberTypeTabs.Add(new MemberTab(pair.Key.Name, pair.Key, pair.Value));
                }
            }
        }

        /// <summary>
        /// A collection of the paths to all the team data files that have recently been loaded.
        /// </summary>
        public HashSet<string> RecentLoadedTeamFilepaths { get; }

        /// <summary>
        /// A collection of the <see cref="MemberTab"/>s.
        /// </summary>
        public ObservableCollection<MemberTab> MemberTypeTabs { get; }

        private readonly MainWindow mainWindowInstance;
        private ObservableCollection<Team> teams;
        private bool isTeamSelected;

        /// <summary>
        /// Initializes a new <see cref="MainWindowDataContext"/>.
        /// </summary>
        public MainWindowDataContext(MainWindow mainWindowInstance)
        {
            this.mainWindowInstance = mainWindowInstance;
            RecentLoadedTeamFilepaths = new HashSet<string>();

            Teams = new ObservableCollection<Team>();
            MemberTypeTabs = new ObservableCollection<MemberTab>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Event for notification of a change of data.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (name != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Adds a new <see cref="Team"/> and focuses it in the editor.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> to add.</param>
        public void AddTeam(Team team)
        {
            Teams.Add(team);
            mainWindowInstance.TeamSelectionComboBox.SelectedItem = team;
        }
    }
}
