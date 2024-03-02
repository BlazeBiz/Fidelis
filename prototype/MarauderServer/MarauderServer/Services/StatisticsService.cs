using MarauderServer.Data;
using MarauderServer.Models;

namespace MarauderServer.Services
{
    public class StatisticsService
    {
        private StatisticsData statisticsData;

        public StatisticsService(StatisticsData statisticsData)
        {
            this.statisticsData = statisticsData;
        }

        public Statistics? GetStatistics()
        {
            return statisticsData.GetStatistics();
        }
    }
}
