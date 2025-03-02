// File: Models/StatisticsViewModel.cs
using System.Collections.Generic;

namespace Gauniv.WebServer.Models
{
    public class StatisticsViewModel
    {
        public int TotalGames { get; set; }
        public IEnumerable<dynamic> GamesPerCategory { get; set; }
        public double AvgGamesPerAccount { get; set; }
        public double AvgTimePlayedPerGame { get; set; }
        public int MaxSimultaneousPlayersOverall { get; set; }
        public int MaxSimultaneousPlayersPerGame { get; set; }
    }
}
