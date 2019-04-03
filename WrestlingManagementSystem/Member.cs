/*
 * Author: Shon Verch
 * File Name: Member.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 04/03/2019
 * Description: The base-class for all members.
 */

using System;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// The base-class for all members.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// The first name of this <see cref="Member"/>.
        /// </summary>
        [MemberProperty(1)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of this <see cref="Member"/>.
        /// </summary>
        [MemberProperty(2)]
        public string LastName { get; set; }

        /// <summary>
        /// The gender of this <see cref="Member"/>.
        /// </summary>
        [MemberProperty(3)]
        public Gender Gender { get; set; }

        /// <summary>
        /// The school that this <see cref="Member"/> is a part of.
        /// </summary>
        [MemberProperty(4)]
        public string School { get; set; }

        /// <summary>
        /// The amount of years of experience that this <see cref="Member"/> has.
        /// </summary>
        [MemberProperty(5)]
        public int YearsOfExperience { get; set; }

        /// <summary>
        /// Initializes a new empty <see cref="Member"/>.
        /// </summary>
        public Member()
        {
            FirstName = LastName = School = string.Empty;
            Gender = Gender.Male;
            YearsOfExperience = 0;
        }

        /// <summary>
        /// Initializes a new <see cref="Member"/>.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="gender"></param>
        /// <param name="school"></param>
        /// <param name="yearsOfExperience"></param>
        public Member(string firstName, string lastName, Gender gender, string school, int yearsOfExperience)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            School = school;
            YearsOfExperience = yearsOfExperience;
        }

        /// <summary>
        /// Retrieves the attributes that will be serialized.
        /// </summary>
        public virtual object[] GetSerializedAttributes() => new object[] {LastName, FirstName, Gender, School, YearsOfExperience};

        /// <summary>
        /// Serializes this <see cref="Member"/> into a set of comma-separated values.
        /// </summary>
        public string Save() => string.Join(",", GetType().Name, string.Join(",", GetSerializedAttributes()));

        /// <summary>
        /// Load a <see cref="Member"/> from a set of comma-separated values.
        /// </summary>
        /// <remarks>
        /// The format consists of n attributes separated by a comma: "attribute1,attribute2,...,attributeN"
        /// </remarks>
        /// <param name="data">The comma-separated <see cref="Member"/> data.</param>
        /// <returns>
        /// A boolean indicating the result of the load.
        /// </returns>
        public virtual bool Load(string data)
        {
            string[] parts = data.Split(',');
            if (parts.Length < 5)
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load member! Missing parameters.");
                return false;
            }

            string lastName = parts[0];
            string firstName = parts[1];
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load member! Missing name parameter.");
                return false;
            }

            if (!Enum.TryParse(parts[2], out Gender gender))
            {
                Logger.LogFunctionEntry(string.Empty, "Failed load member! Could not convert gender value.", 
                    LoggerVerbosity.Error);

                return false;
            }

            string school = parts[3];
            if (string.IsNullOrEmpty(school))
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load member! Missing school parameter.");
                return false;
            }

            if (!int.TryParse(parts[4], out int experience))
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load member! Could not convert years of experience.");
                return false;
            }

            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            School = school;
            YearsOfExperience = experience;
            return true;
        }
    }
}
