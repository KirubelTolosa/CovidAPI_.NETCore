using Covid.BLL.Service.Models;
using Covid.BLL.Service.Utils;
using Covid.DAL.Service.Models;
using System;
using System.Collections.Generic;

namespace Covid.BLL.Service
{
    public interface ICovidBLService
    {
        List<CovidGlobalCaseCountDADto> FetchGlobalCasesFromAPI(Metrics metrics, Locations location);
        List<CovidGlobalCaseCountDADto> FetchCasesFromFile(Metrics metrics, Locations locations);
        (List<CovidLocationDADto>, string[]) FetchLocationsFromFile();
        void InsertLocations(Locations? locations);
        List<CovidLocations_USDADto> FetchUSLocationsFromAPI();
        void InsertCases(Metrics metrics, Locations locations);
        List<NationalCasesBLDto> GetCountOfCasesByCountry(Metrics metrics, string Country, DateTime? Date);
        List<USCountyCaseCountBLDto> GetCountOfUSCasesByCounty(Metrics metrics, Locations locations, string State, string County, DateTime? Date);
        List<USStateCaseCountBLDto> GetCountOfUSCasesByState(Metrics metrics, Locations locations, string State, DateTime? Date);
        List<GlobalTotalCountsBLDto> GetGlobalTotalCounts(Metrics metrics);
        List<DailyCaseCountsBLDto> GetDailyCaseCountsByCountry(Metrics metrics, string Country);
        List<NationalCasesBLDto> GetCountOfCasesForAllNations(Metrics metrics);
        List<LocationNameBLDto> GetListOfLocations(Locations locations, string region);
    }
}
