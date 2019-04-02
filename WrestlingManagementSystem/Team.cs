/*
 * Author: Shon Verch
 * File Name: Team.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 04/01/2019
 * Description: DESCRIPTION
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WrestlingManagementSystem
{
    public class Team
    {
        /// <summary>
        /// The name of the team.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Member"/>s in this <see cref="Team"/>.
        /// </summary>
        public ObservableCollection<Member> Members { get; set; }

        /// <summary>
        /// Retrieves the number of <see cref="Wrestler"/> members on this <see cref="Team"/>.
        /// </summary>
        public int WrestlerCount => GetMemberCountOfType<Wrestler>();

        /// <summary>
        /// Retrieves the number of <see cref="Wrestler"/> members on this <see cref="Team"/> that are <see cref="Gender.Male"/>.
        /// </summary>
        public int MaleWrestlerCount => GetMemberCountOfTypeGender<Wrestler>(Gender.Male);

        /// <summary>
        /// Retrieves the number of <see cref="Wrestler"/> members on this <see cref="Team"/> that are <see cref="Gender.Female"/>.
        /// </summary>
        public int FemaleWrestlerCount => GetMemberCountOfTypeGender<Wrestler>(Gender.Female);

        /// <summary>
        /// Retrieves the number of <see cref="Coach"/> members on this <see cref="Team"/>.
        /// </summary>
        public int CoachCount => GetMemberCountOfType<Coach>();

        /// <summary>
        /// Retrieves the number of <see cref="Coach"/> members on this <see cref="Team"/> that are <see cref="Gender.Male"/>.
        /// </summary>
        public int MaleCoachCount => GetMemberCountOfTypeGender<Coach>(Gender.Male);

        /// <summary>
        /// Retrieves the number of <see cref="Coach"/> members on this <see cref="Team"/> that are <see cref="Gender.Female"/>.
        /// </summary>
        public int FemaleCoachCount => GetMemberCountOfTypeGender<Coach>(Gender.Female);

        /// <summary>
        /// Retrieves the number of <see cref="Coach"/> members on this <see cref="Team"/> that are of the specified <see cref="CoachType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetCoachCountOfType(CoachType type) => GetMembersOfType<Coach>().Count(member => ((Coach) member).Type == type);

        /// <summary>
        /// Retrieves all the <see cref="Member"/>s on this <see cref="Team"/> that are of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Member"/> subclass.</typeparam>
        /// <returns></returns>
        public IEnumerable<Member> GetMembersOfType<T>() where T : Member => Members.Where(member => member is T);

        /// <summary>
        /// Retrieves the number of members on this <see cref="Team"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Member"/> subclass.</typeparam>
        /// <returns>An <see cref="int"/> indicating the number of members on this <see cref="Team"/> of type <typeparamref name="T"/></returns>
        public int GetMemberCountOfType<T>() where T : Member => GetMembersOfType<T>().Count();

        /// <summary>
        /// Retrieves the number of type <typeparamref name="T"/> members on this <see cref="Team"/> that are of the specified <see cref="Gender"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Member"/> subclass.</typeparam>
        /// <param name="gender">The <see cref="Gender"/> to filter.</param>
        /// <returns>
        /// An <see cref="int"/> indicating the number of type <typeparamref name="T"/> members on this <see cref="Team"/>
        /// that are of the specified <see cref="Gender"/>.
        /// </returns>
        public int GetMemberCountOfTypeGender<T>(Gender gender) where T : Member => GetMembersOfType<T>().Count(member => member.Gender == gender);

        /// <summary>
        /// Retrieves the total number of matches between all the <see cref="Wrestler"/> members on this <see cref="Team"/>.
        /// </summary>
        public int TotalMatches => GetMembersOfType<Wrestler>().Sum(member => ((Wrestler) member).TotalMatches);

        /// <summary>
        /// Retrieves the total number of wins by the <see cref="Wrestler"/> members on this <see cref="Team"/>.
        /// </summary>
        public int TotalWins => GetMembersOfType<Wrestler>().Sum(member => ((Wrestler) member).Wins);

        /// <summary>
        /// Retrieves the total number of losses by the <see cref="Wrestler"/> members on this <see cref="Team"/>.
        /// </summary>
        public int TotalLosses => GetMembersOfType<Wrestler>().Sum(member => ((Wrestler) member).Losses);

        /// <summary>
        /// Retrieves the total wins to total matches percentage for this <see cref="Team"/>.
        /// </summary>
        public float WinPercentage => TotalWins / (float)TotalMatches * 100.0f;

        /// <summary>
        /// Retrieves the total losses to total matches percentage for this <see cref="Team"/>.
        /// </summary>
        public float LossPercentage => TotalLosses / (float)TotalMatches * 100.0f;

        /// <summary>
        /// The total number of points for this <see cref="Team"/>.
        /// </summary>
        public int TotalPoints => GetMembersOfType<Wrestler>().Sum(member => ((Wrestler) member).TotalPoints);

        /// <summary>
        /// The total number of wins by pin by all the <see cref="Wrestler"/> members on this <see cref="Team"/>.
        /// </summary>
        public int TotalPinCount => GetMembersOfType<Wrestler>().Sum(member => ((Wrestler) member).WinsByPin);

        /// <summary>
        /// The average points per match for this <see cref="Team"/>.
        /// </summary>
        public float AveragePointsPerMatch => TotalPoints / (float)TotalMatches;

        /// <summary>
        /// Initializes a new <see cref="Team"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="Team"/>.</param>
        public Team(string name)
        {
            Name = name;
            Members = new ObservableCollection<Member>();
        }
    }
}
