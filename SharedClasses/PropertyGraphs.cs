using System;
using System.Linq;

namespace SharedClasses
{
    public static class PropertyGraphs
    {
        /// <summary>
        /// Takes the last 5 months and places them into a comma separated string for graphs
        /// </summary>
        /// <returns></returns>
        public static string LoadLabels()
        {
            string Labels;

            DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
            DateTime endDate = DateTime.Now.Date;

            var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("MMM")).ToList();
            Labels = String.Join("','", months);
            Labels = "'" + Labels + "'";
            return Labels;
        }
    }
}