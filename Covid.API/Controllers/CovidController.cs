using CovidAPI.Models;
using Covid.BLL.Service;
using Covid.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Covid.API.Controllers
{ 

    [ApiController]
    [Route("[controller]")]
    public class CovidController : ControllerBase
    {
        private readonly ICovidBLService _covidBLService;
        public CovidController(ICovidBLService covidBLService)
        {
            this._covidBLService = covidBLService;
        }        
        /// <summary>
        /// Get count of cases of each nation in the world
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        [HttpGet("CasesByNations")]
        public IEnumerable<NationalCasesDataModel> Get([FromQuery] Metrics metrics)
        {
            return _covidBLService.GetCountOfCasesForAllNations(metrics.ToBLLMetrics()).Select(Item => new NationalCasesDataModel() { Count = Item.Count, Country = Item.Country }).ToList();
        }

        /// <summary>
        /// Get the total count of cases worldwide
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        [HttpGet("GlobalCount")]
        public IEnumerable<GlobalTotalCountsDataModel> GetGlobalCaseCounts([FromQuery] Metrics metrics)
        {
            return _covidBLService.GetGlobalTotalCounts(metrics.ToBLLMetrics()).Select(Item => new GlobalTotalCountsDataModel() {
                GlobalCases = (metrics == Metrics.CONFIRMED_CASES) ? "Tested Positive" : (metrics == Metrics.DEATHS) ? "Deceased" : (metrics == Metrics.RECOVERIES) ? "Recovered" : "Null", Count = Item.Count }).ToList();
        }

        /// <summary>
        /// Get count of cases by country
        /// </summary>
        /// <remarks>
        /// If date is not specified the latest count is returned 
        /// </remarks>
        /// <param name="metrics"></param>
        /// <param name="Country"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("national/{Country}")]
        public List<NationalCasesDataModel> GetCountryCaseCounts([FromQuery] Metrics metrics, string Country, DateTime? date)
        {
            if (date == null)
                date = DateTime.MinValue;
            return _covidBLService.GetCountOfCasesByCountry(metrics.ToBLLMetrics(), Country, date).Select(Item => new NationalCasesDataModel() { Country = Item.Country, Count = Item.Count }).ToList();
        }

        /// <summary>
        /// Get the daily count of cases in a country
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="Country"></param>
        /// <returns></returns>
        [HttpGet("national/DailyCount")]
        public List<DailyCaseCountsDataModel> GetDailyCountOfCasesByCountry([FromQuery] Metrics metrics, string Country)
        {
            return _covidBLService.GetDailyCaseCountsByCountry(metrics.ToBLLMetrics(), Country).Select(Item => new DailyCaseCountsDataModel() { Date = Item.Date.ToShortDateString(), Count = Item.Count }).ToList();
        }
        /// <summary>
        /// Get the count of cases in a US county
        /// </summary>
        /// <remarks>
        /// If date is not specified the latest count is returned 
        /// </remarks>
        /// <param name="metrics"></param>
        /// <param name="State"></param>
        /// <param name="County"></param>
        /// <param name="date"></param>
        /// <returns></returns>        
        
        [HttpGet("us/{State}/{County}")]
        public List<USCountyCaseCountDataModel> GetUSCountyCaseCounts([FromQuery] Metrics metrics, string State, string County, DateTime? date = null)
        {
            Locations locations = Locations.US;
            if (date == null)
                date = DateTime.MinValue;
            return _covidBLService.GetCountOfUSCasesByCounty(metrics.ToBLLMetrics(), locations.ToBLLocations(), State, County, date).Select(Item => new USCountyCaseCountDataModel() { State = Item.State, County = Item.County, Date = Item.Date.ToShortDateString(), Count = Item.Count }).ToList();
        }

        /// <summary>
        /// Get the count cases in a US State
        /// </summary>
        /// <remarks>
        /// If date is not specified the latest count is returned 
        /// </remarks>
        /// <param name="metrics"></param>
        /// <param name="State"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("us/{State}")]
        public List<USStateCaseCountDataModel> GetUSStateCaseCounts([FromQuery] Metrics metrics, string State, DateTime? date)
        {
            Locations locations = Locations.US;
            if (date == null)
                date = DateTime.MinValue;
            return _covidBLService.GetCountOfUSCasesByState(metrics.ToBLLMetrics(), locations.ToBLLocations(), State, date).Select(Item => new USStateCaseCountDataModel() { State = Item.State, Date = Item.Date.ToShortDateString(), Count = Item.Count }).ToList();
        }
        /// <summary>
        /// Get the list of location names 
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="region"></param>
        /// <remarks>Use 'nation' for Global, Use 'state' or 'county' for US (all in lowercase letters)</remarks>
        /// <returns></returns>
        [HttpGet("locations/{region}")]
        public List<LocationNameDataModel> GetListOfLocations([FromQuery] Locations locations, string region)
        {
            return _covidBLService.GetListOfLocations(locations.ToBLLocations(), region).Select(Item => new LocationNameDataModel() { location = Item.location }).ToList();
        }
    }
}
