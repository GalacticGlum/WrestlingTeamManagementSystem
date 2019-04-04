/*
 * Author: Shon Verch
 * File Name: Coach.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 04/03/2019
 * Description: The coach data class.
 */

using System.Collections.Generic;
using System.Linq;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// The coach data class.
    /// </summary>
    public class Coach : Member
    {
        /// <summary>
        /// The tag string indicating the member type.
        /// </summary>
        public const string SerializationTypeTag = "Coach";

        /// <summary>
        /// The type of <see cref="Coach"/>.
        /// </summary>
        [MemberProperty(7)]
        public CoachType Type { get; set; }

        /// <summary>
        /// Initializes an empty <see cref="Coach"/>.
        /// </summary>
        public Coach()
        {
            Type = CoachType.HandsOn;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="T:WrestlingManagementSystem.Coach" />.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="gender"></param>
        /// <param name="school"></param>
        /// <param name="yearsOfExperience"></param>
        public Coach(string firstName, string lastName, Gender gender, string school, int yearsOfExperience) : 
            base(firstName, lastName, gender, school, yearsOfExperience)
        {
        }

        /// <summary>
        /// Retrieves the attributes that will be serialized.
        /// </summary>
        public override object[] GetSerializedAttributes()
        {
            List<object> attributes = base.GetSerializedAttributes().ToList();
            attributes.AddRange(new object[] {ConvertCoachType(Type)});

            return attributes.ToArray();
        }

        /// <inheritdoc />
        /// <summary>
        /// Load a <see cref="Coach" /> from a set of comma-separated values.
        /// </summary>
        /// <param name="data">The comma-separated <see cref="Coach" /> data.</param>
        /// <returns>
        /// A boolean indicating the result of the load.
        /// </returns>
        public override bool Load(string data)
        {
            if (!base.Load(data)) return false;

            string[] parts = data.Split(',');
            if (parts.Length < 6)
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load coach! Missing parameters.");
                return false;
            }

            switch (parts[5])
            {
                case "Hands-on":
                    Type = CoachType.HandsOn;
                    break;
                case "Support":
                    Type = CoachType.Support;
                    break;
                default:
                    Logger.LogFunctionEntry(string.Empty, "Failed to load coach! Could not convert coach type.");
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Convert the coach type into a serialized-safe format.
        /// </summary>
        /// <param name="coachType">The coach type.</param>
        /// <returns></returns>
        private static string ConvertCoachType(CoachType coachType)
        {
            switch (coachType)
            {
                case CoachType.HandsOn:
                    return "Hands-on";
                case CoachType.Support:
                    return "Support";
            }

            // If this happens, there is something terribly wrong.
            return "NULL";
        }
    }
}
