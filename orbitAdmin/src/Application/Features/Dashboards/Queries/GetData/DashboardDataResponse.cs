using System.Collections.Generic;

namespace SchoolV01.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {


    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }
    public class MyChartSeries
    {

        public string Label { get; set; }
        public double Data { get; set; }
    }

}