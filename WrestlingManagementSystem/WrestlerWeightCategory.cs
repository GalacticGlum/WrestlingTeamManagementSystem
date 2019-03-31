/*
 * Author: Shon Verch
 * File Name: WrestlerWeightCategory.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 03/31/2019
 * Description: DESCRIPTION
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace WrestlingManagementSystem
{
    public struct WrestlerWeightCategory
    {
        [JsonProperty("Gender", Required = Required.Always)]
        public Gender CategoryGender { get; }

        [JsonProperty("Weight", Required = Required.Always)]
        public List<float> Weights { get; }
    }
}
