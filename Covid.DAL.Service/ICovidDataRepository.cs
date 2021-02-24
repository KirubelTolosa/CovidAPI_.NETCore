using Covid.DAL.Service.Models;
using Covid.DAL.Service.Utils;
using System;
using System.Collections.Generic;



namespace Covid.DAL.Service
{
    public interface ICovidDataRepository
    {
        void InsertGlobalLocations(List<CovidLocationDADto> dALRecords);
        void InsertUSLocations(List<CovidLocations_USDADto> dALRecords);
        void InsertGlobalCases(List<CovidGlobalCaseCountDADto> dALRecords, Metrics metrics);
        void InsertUSCases(List<CovidUSCaseCountDADto> dALRecords, Metrics metrics);
        DateTime GetLastUpdateDate(Metrics metrics, Locations locations);
        List<NationalCasesDADto> GetCountOfCasesForAllNations(Metrics metrics);
        List<NationalCasesDADto> GetCountOfCasesByCountry(Metrics metrics, string Country, DateTime? Date);
        List<USCountyCaseCountDADto> GetCountOfUSCasesByCounty(Metrics metrics, Locations location, string State, string County, DateTime? Date);
        List<USStateCaseCountDADto> GetCountOfUSCasesByState(Metrics metrics, Locations locations, string State, DateTime? Date);
        List<DailyCaseCountsDADto> GetDailyCaseCountsByCountry(Metrics metrics, string Country);
        List<GlobalTotalCountsDADto> GetGlobalTotalCounts(Metrics metrics);
        Dictionary<string, int> GetLocationTableCompositeKeys();
        List<LocationNameDADto> GetListOfLocations(Locations locations, string region);
    }
}
