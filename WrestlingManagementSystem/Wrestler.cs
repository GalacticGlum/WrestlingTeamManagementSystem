/*
 * Author: Shon Verch
 * File Name: Wrestler.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 03/27/2019
 * Description: DESCRIPTION
 */

using System;
using System.Collections.Generic;

namespace WrestlingManagementSystem
{
    public class Wrestler : Member
    {
        public DateTime Birthdate { get; private set; }
        public float Weight { get; private set; }

        public int Wins { get; private set; }
        public int WinsByPin { get; private set; }

        public int Losses { get; private set; }
        public int TotalPoints { get; private set; }

        public WrestlerStatus Status { get; private set; }
        public bool IsUnfiformSignedOut { get; private set; }

        public int TotalMatches => Wins + Losses;
        public float PercentWinLoss => Wins / (float)TotalMatches * 100.0f;
        public float AverageMatchPoints => TotalPoints / (float)TotalMatches;
        public float WeightCategory
        {
            get
            {
               
            }
        }

        private List<Tuple<string, List<float>>>
    }
}
