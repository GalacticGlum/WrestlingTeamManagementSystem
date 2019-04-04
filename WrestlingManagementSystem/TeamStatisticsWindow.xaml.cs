/*
 * Author: Shon Verch
 * File Name: TeamStatisticsWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: Interaction logic for TeamStatisticsWindow.xaml
 */

using System.Windows;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// Interaction logic for TeamStatisticsWindow.xaml
    /// </summary>
    public partial class TeamStatisticsWindow : Window
    {
        /// <summary>
        /// The team to display statistics for.
        /// </summary>
        public Team Team { get; }

        /// <summary>
        /// Initialize a new <see cref="TeamStatisticsWindow"/>.
        /// </summary>
        /// <param name="team">The team to display statistics for.</param>
        public TeamStatisticsWindow(Team team)
        {
            InitializeComponent();
            Team = team;

            DataContext = this;

            // Initialize the statistics values
            MemberCount.Content = team.TotalMembers;
            WrestlerCount.Content = team.WrestlerCount;
            MaleWrestlerCount.Content = team.MaleWrestlerCount;
            FemaleWrestlerCount.Content = team.FemaleWrestlerCount;
            CoachCount.Content = team.CoachCount;
            HandsOnCoachCount.Content = team.GetCoachCountOfType(CoachType.HandsOn);
            SupportCoachCount.Content = team.GetCoachCountOfType(CoachType.Support);
            MaleCoachCount.Content = team.MaleCoachCount;
            FemaleCoachCount.Content = team.FemaleCoachCount;
            TeamTotalNumberofWins.Content = team.TotalWins;
            TeamTotalNumberofLosses.Content = team.TotalLosses;
            TeamWinPercentage.Content = team.WinPercentage;
            TeamLossPercentage.Content = team.LossPercentage;
            TeamTotalPointCount.Content = team.TotalPoints;
            TeamTotalPinCount.Content = team.TotalPinCount;
            TeamTotalNumberofMatches.Content = team.TotalMatches;
            TeamAveragePointsPerMatch.Content = team.AveragePointsPerMatch;
        }
    }
}
