/*
 * Author: Shon Verch
 * File Name: Member.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 04/01/2019
 * Description: DESCRIPTION
 */

using System;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    public class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        public string School { get; set; }
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
