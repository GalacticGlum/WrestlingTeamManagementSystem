/*
 * Author: Shon Verch
 * File Name: TeamStatisticsWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: Interaction logic for TeamStatisticsWindow.xaml
 */

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WrestlingManagementSystem
{
    public struct TeamStatisticsBreakdownEntry
    {
        public float WeightCategory { get; }
        public int AllCount { get; }
        public int MaleCount { get; }
        public int FemaleCount { get; }

        public TeamStatisticsBreakdownEntry(float weightCategory, int allCount, int maleCount, int femaleCount)
        {
            WeightCategory = weightCategory;
            AllCount = allCount;
            MaleCount = maleCount;
            FemaleCount = femaleCount;
        }
    }

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

            List<float> allWeightCategories = new List<float>();

            // Build a list of all the weight categories---occuring once at most.
            foreach (KeyValuePair<Gender, WeightCategoryCollection> weightCategoryCollection in Wrestler
                .WeightCategories)
            {
                foreach (float valueWeight in weightCategoryCollection.Value.Weights)
                {
                    if (allWeightCategories.Contains(valueWeight)) continue;
                    allWeightCategories.Add(valueWeight);
                }
            }

            allWeightCategories.Sort();

            // Build the data grid
            WeightCategoryBreakdownDataGrid.Columns.Clear();
            WeightCategoryBreakdownDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Weight Category",
                Binding = new Binding("WeightCategory")
            });

            WeightCategoryBreakdownDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "All",
                Binding = new Binding("AllCount")
            });

            WeightCategoryBreakdownDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Male",
                Binding = new Binding("MaleCount")
            });

            WeightCategoryBreakdownDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Female",
                Binding = new Binding("FemaleCount")
            });

            // Build the all breakdown row.
            IEnumerable<Member> allWrestlers = team.GetMembersOfType<Wrestler>();
            IEnumerable<Member> maleWrestlers =
                team.GetMembersOfType<Wrestler>().Where(member => member.Gender == Gender.Male);
            IEnumerable<Member> femaleWrestlers =
                team.GetMembersOfType<Wrestler>().Where(member => member.Gender == Gender.Female);

            List<TeamStatisticsBreakdownEntry> entries = new List<TeamStatisticsBreakdownEntry>();
            foreach (float category in allWeightCategories)
            {
                bool WeightCategoryCountPredicate(Member member) => ((Wrestler) member).WeightCategory == category;

                int allCount = allWrestlers.Count(WeightCategoryCountPredicate);
                int maleCount = maleWrestlers.Count(WeightCategoryCountPredicate);
                int femaleCount = femaleWrestlers.Count(WeightCategoryCountPredicate);

                // Create the entry
                TeamStatisticsBreakdownEntry entry = new TeamStatisticsBreakdownEntry(category, allCount, maleCount, femaleCount);
                entries.Add(entry);
            }

            WeightCategoryBreakdownDataGrid.ItemsSource = entries;
        }
    }
}
