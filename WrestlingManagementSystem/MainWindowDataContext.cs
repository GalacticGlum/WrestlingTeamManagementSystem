/*
 * Author: Shon Verch
 * File Name: MainWindowDataContext.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 04/01/2019
 * Description: Stores all data regarding the MainWindow.
 *              It is notified when data is changed and updates data accordingly.
 */

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
        private ObservableCollection<Team> teams;

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
        /// Initializes a new <see cref="MainWindowDataContext"/>.
        /// </summary>
        public MainWindowDataContext()
        {
            Teams = new ObservableCollection<Team>();
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
    }
}
