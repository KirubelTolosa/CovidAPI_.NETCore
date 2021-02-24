using Covid.BLL.Service.Models;
using Covid.DAL.Service.Models;
using System.Collections.Generic;

namespace Covid.API.Utils
{
    public static class Extensions
    {
        private static readonly Dictionary<Covid.API.Utils.Metrics, Covid.BLL.Service.Utils.Metrics> MetricsMap = new Dictionary<Covid.API.Utils.Metrics, Covid.BLL.Service.Utils.Metrics>
        {
            {
                Metrics.CONFIRMED_CASES, BLL.Service.Utils.Metrics.CONFIRMED_CASES
            },
            {
                Metrics.DEATHS, BLL.Service.Utils.Metrics.DEATHS
            },
            {
                Metrics.RECOVERIES, BLL.Service.Utils.Metrics.RECOVERIES
            }
        };
        private static readonly Dictionary<Covid.API.Utils.Locations, Covid.BLL.Service.Utils.Locations> LocationsMap = new Dictionary<Covid.API.Utils.Locations, Covid.BLL.Service.Utils.Locations>
        {
            {
                Locations.Global, BLL.Service.Utils.Locations.Global
            },
            {
                Locations.US, BLL.Service.Utils.Locations.US
            }
        };
        internal static Covid.BLL.Service.Utils.Metrics ToBLLMetrics(this Metrics metrics)
        {
            return MetricsMap[metrics];
        }
        internal static Covid.BLL.Service.Utils.Locations ToBLLocations(this Locations locations)
        {
            return LocationsMap[locations];
        }
    }
}
