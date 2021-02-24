using Covid.BLL.Service.Models;
using Covid.DAL.Service.Models;
using System.Collections.Generic;

namespace Covid.BLL.Service.Utils
{
    public static partial class Extensions
    {
        public static (Covid.DAL.Service.Utils.Metrics, Covid.DAL.Service.Utils.Locations) GetDALEnums()
        {
            return (new Covid.DAL.Service.Utils.Metrics(), new Covid.DAL.Service.Utils.Locations());

        }



        private static readonly Dictionary<Covid.BLL.Service.Utils.Metrics, Covid.DAL.Service.Utils.Metrics> MetricsMap = new Dictionary<Covid.BLL.Service.Utils.Metrics, Covid.DAL.Service.Utils.Metrics>
        {
            {
                Metrics.CONFIRMED_CASES, DAL.Service.Utils.Metrics.CONFIRMED_CASES
            },
            {
                Metrics.DEATHS, DAL.Service.Utils.Metrics.DEATHS
            },
            {
                Metrics.RECOVERIES, DAL.Service.Utils.Metrics.RECOVERIES
            }
        };
        private static readonly Dictionary<Covid.BLL.Service.Utils.Locations, Covid.DAL.Service.Utils.Locations> LocationsMap = new Dictionary<Covid.BLL.Service.Utils.Locations, Covid.DAL.Service.Utils.Locations>
        {
            {
                Locations.Global, DAL.Service.Utils.Locations.Global
            },
            {
                Locations.US, DAL.Service.Utils.Locations.US
            }
        };
        internal static Covid.DAL.Service.Utils.Metrics ToDALMetrics(this Metrics metrics)
        {
            return MetricsMap[metrics];
        }
        internal static Covid.DAL.Service.Utils.Locations ToDALocations(this Locations locations)
        {
            return LocationsMap[locations];
        }
    }
}
