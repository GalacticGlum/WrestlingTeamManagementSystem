/*
 * Author: Shon Verch
 * File Name: Wrestler.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 04/03/2019
 * Description: The wrestler data class.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// The wrestler data class.
    /// </summary>
    public class Wrestler : Member
    {
        /// <summary>
        /// The tag string indicating the member type.
        /// </summary>
        public const string SerializationTypeTag = "Wrestler";

        /// <summary>
        /// The number of comma-separated values in a wrestler data line.
        /// </summary>
        private const int SerializationParameterLength = 14;

        private static Dictionary<Gender, WeightCategoryCollection> weightCategoriesCache;

        /// <summary>
        /// Retrieve all the weight categories mapped to gender.
        /// </summary>
        public static Dictionary<Gender, WeightCategoryCollection> WeightCategories
        {
            get
            {
                if (weightCategoriesCache != null) return weightCategoriesCache;

                // Load the weight categories from resources
                Uri weightCategoriesJsonUri = new Uri("/WeightCategories.json", UriKind.Relative);
                StreamResourceInfo resourceStreamInfo = Application.GetResourceStream(weightCategoriesJsonUri);

                // Make sure that our resource stream is available.
                Logger.Assert(resourceStreamInfo != null, "Resource by name \'WeightCategories.json\' " +
                                                          "not found or could not be loaded", LoggerDestination.Form);

                weightCategoriesCache = new Dictionary<Gender, WeightCategoryCollection>();
                using (StreamReader reader = new StreamReader(resourceStreamInfo.Stream))
                {
                    string jsonSource = reader.ReadToEnd();
                    WeightCategoryCollection[] categories = JsonConvert.DeserializeObject<WeightCategoryCollection[]>(jsonSource);
                    foreach (WeightCategoryCollection category in categories)
                    {
                        // If a category with the specified gender already exists in our
                        // dictionary cache, we simply extend the cached list in our dictionary
                        if (weightCategoriesCache.ContainsKey(category.Gender))
                        {
                            weightCategoriesCache[category.Gender].Weights.AddRange(category.Weights);
                        }
                        else
                        {
                            weightCategoriesCache[category.Gender] = category;
                        }
                    }
                }

                return weightCategoriesCache;
            }
        }

        /// <summary>
        /// The date of birth of this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(7)]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Retrieves this <see cref="Wrestler"/>'s birthdate as a formatted string.
        /// </summary>
        /// <remarks>
        /// The format is MM/dd/yyyy
        /// </remarks>
        public string BirthdateFormatted => $"{Birthdate:MM/dd/yyyy}";

        /// <summary>
        /// The weight of this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(8)]
        public float Weight { get; set; }

        /// <summary>
        /// The number of wins for this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(10)]
        public int Wins { get; set; }

        /// <summary>
        /// The number of losses for this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(11)]
        public int Losses { get; set; }

        /// <summary>
        /// The number of wins by pin (technique) for this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(12)]
        public int WinsByPin { get; set; }

        /// <summary>
        /// The total number of points achieved by this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(13)]
        public int TotalPoints { get; set; }

        /// <summary>
        /// The status of this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(14)]
        public WrestlerStatus Status { get; set; }

        /// <summary>
        /// Whether this <see cref="Wrestler"/> signed out a uniform.
        /// </summary>
        [MemberProperty(15)]
        public bool IsUnfiformSignedOut { get; set; }

        /// <summary>
        /// The total number of matches fought by this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(16, isReadonly: true)]
        public int TotalMatches => Wins + Losses;

        /// <summary>
        /// The percentage of wins to total matches by this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(17, isReadonly: true)]
        public float WinPercentage => Wins / (float)TotalMatches * 100.0f;

        /// <summary>
        /// The percentage of losses to total matches by this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(18, isReadonly: true)]
        public float LossPercentage => Losses / (float)TotalMatches * 100.0f;

        /// <summary>
        /// The average points per match for this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(19, isReadonly: true)]
        public float AverageMatchPoints => TotalPoints / (float)TotalMatches;

        /// <summary>
        /// The weight category of this <see cref="Wrestler"/>.
        /// </summary>
        [MemberProperty(9, isReadonly: true)]
        public float WeightCategory
        {
            get
            {
                float weightCategory = float.MinValue;
                foreach (float currentWeightCategory in WeightCategories[Gender].Weights)
                {
                    if (Weight < currentWeightCategory)
                    {
                        weightCategory = Math.Max(weightCategory, currentWeightCategory);
                    }
                }

                List<float> candidateCategories = new List<float>();
                foreach (float currentWeightCategory in WeightCategories[Gender].Weights)
                {
                    if (Weight <= currentWeightCategory)
                    {
                        candidateCategories.Add(currentWeightCategory);
                    }
                }

                // If there are no candidate categories (i.e. the weight of the wrestler is not less than any category),
                // that means that wrestler exceeds the largest weight category so they should be placed in that one; otherwise,
                // select the smallest candidate category.
                return candidateCategories.Count == 0 ? WeightCategories[Gender].Weights.Max() : candidateCategories.Min();
            }
        }

        /// <summary>
        /// Retrieves the attributes that will be serialized.
        /// </summary>
        public override object[] GetSerializedAttributes()
        {
            List<object> attributes = base.GetSerializedAttributes().ToList();
            attributes.AddRange(new object[]
            {
                BirthdateFormatted, Weight, WeightCategory, Wins,
                Losses, TotalPoints, WinsByPin, Status, IsUnfiformSignedOut
            });

            return attributes.ToArray();
        }

        /// <summary>
        /// Load a <see cref="Wrestler"/> from a set of comma-separated values.
        /// </summary>
        /// <remarks>
        /// The format consists of n attributes separated by a comma: "attribute1,attribute2,...,attributeN"
        /// </remarks>
        /// <param name="data">The comma-separated <see cref="Wrestler"/> data.</param>
        /// <returns>
        /// A boolean indicating the result of the load.
        /// </returns>
        public override bool Load(string data)
        {
            if (!base.Load(data)) return false;

            string[] parts = data.Split(',');
            if (parts.Length < SerializationParameterLength)
            {
                Logger.LogFunctionEntry(string.Empty, "Failed to load wrestler! Missing parameters.");
                return false;
            }

            if (!DateTime.TryParseExact(parts[5], "MM/dd/yyyy", CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime birthdate))
            {
                Logger.LogFunctionEntry(string.Empty, "Invalid datetime format.");
                return false;
            }

            if (!float.TryParse(parts[6], out float weight))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert weight to float.");
                return false;
            }

            if (!float.TryParse(parts[7], out float weightCategory))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert weight category to float.");
                return false;
            }

            if (!int.TryParse(parts[8], out int wins))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert wins to int.");
                return false;
            }

            if (!int.TryParse(parts[9], out int losses))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert losses to int.");
                return false;
            }

            if (!int.TryParse(parts[10], out int totalPoints))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert total points to int.");
                return false;
            }

            if (!int.TryParse(parts[11], out int winsByPin))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert wins by pin to int.");
                return false;
            }

            if (!Enum.TryParse(parts[12], out WrestlerStatus wrestlerStatus))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert wrestler status to enum.");
                return false;
            }

            if (!bool.TryParse(parts[13], out bool isUnfiformSignedOut))
            {
                Logger.LogFunctionEntry(string.Empty, "Could not convert uniform signed out to bool.");
                return false;
            }

            Birthdate = birthdate;
            Weight = weight;
            Wins = wins;
            Losses = losses;
            TotalPoints = totalPoints;
            WinsByPin = winsByPin;
            Status = wrestlerStatus;
            IsUnfiformSignedOut = isUnfiformSignedOut;

            return true;
        }
    }
}
