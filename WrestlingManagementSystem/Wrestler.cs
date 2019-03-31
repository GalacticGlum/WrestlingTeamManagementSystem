/*
 * Author: Shon Verch
 * File Name: Wrestler.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 03/31/2019
 * Description: DESCRIPTION
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;
using WrestlingManagementSystem.Logging;

namespace WrestlingManagementSystem
{
    public class Wrestler : Member
    {
        private static Dictionary<Gender, WeightCategoryCollection> weightCategoriesCache;
        private static Dictionary<Gender, WeightCategoryCollection> WeightCategories
        {
            get
            {
                if (weightCategoriesCache != null) return weightCategoriesCache;

                // Load the weight categories from resources
                Uri weightCategoriesJsonUri = new Uri("/WeightCategories.json", UriKind.Relative);
                StreamResourceInfo resourceStreamInfo = Application.GetResourceStream(weightCategoriesJsonUri);

                // Make sure that our resource stream is available.
                Logger.Assert(resourceStreamInfo != null, "Resource by name \'WeightCategories.json\' not found or could not be loaded", LoggerDestination.Form);

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


        public DateTime Birthdate { get; private set; }
        public float Weight { get; private set; }

        public int Wins { get; private set; }
        public int WinsByPin { get; private set; }

        public int Losses { get; private set; }
        public int TotalPoints { get; private set; }

        public WrestlerStatus Status { get; private set; }
        public bool IsUnfiformSignedOut { get; private set; }

        public int TotalMatches => Wins + Losses;
        public float WinPercentage => Wins / (float)TotalMatches * 100.0f;
        public float LossPercentage => Losses / (float)TotalMatches * 100.0f;
        public float AverageMatchPoints => TotalPoints / (float)TotalMatches;
        public float WeightCategory
        {
            get
            {
                float weightCategory = float.MinValue;
                foreach (float currentWeightCategory in weightCategoriesCache[Gender].Weights)
                {
                    if (Weight < currentWeightCategory)
                    {
                        weightCategory = Math.Max(weightCategory, currentWeightCategory);
                    }
                }

                List<float> candidateCategories = new List<float>();
                foreach (float currentWeightCategory in weightCategoriesCache[Gender].Weights)
                {
                    if (Weight <= currentWeightCategory)
                    {
                        candidateCategories.Add(currentWeightCategory);
                    }
                }

                // If there are no candidate categories (i.e. the weight of the wrestler is not less than any category),
                // that means that wrestler exceeds the largest weight category so they should be placed in that one; otherwise,
                // select the smallest candidate category.
                return candidateCategories.Count == 0 ? weightCategoriesCache[Gender].Weights.Max() : candidateCategories.Min();
            }
        }
    }
}
