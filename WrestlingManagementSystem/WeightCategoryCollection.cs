/*
 * Author: Shon Verch
 * File Name: WrestlerWeightCategory.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 03/31/2019
 * Description: A discrete collection of weight categories for a specified Gender.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// A discrete collection of weight categories for a specified <see cref="WrestlingManagementSystem.Gender"/>.
    /// </summary>
    public struct WeightCategoryCollection
    {
        [JsonProperty("Gender", Required = Required.Always)]
        public Gender Gender { get; set; }

        [JsonProperty("Weights", Required = Required.Always)]
        public List<float> Weights { get; set; }
    }
}
