using Covid.BLL.Service.Models;
using Covid.BLL.Service;
using Covid.DAL.Service;
using CsvHelper;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Covid.BLL.Service.Utils;
using Covid.DAL.Service.Models;

namespace Covid.BLL.Service
{
    public class CovidBLService : ICovidBLService
    {
        private string path;
        private ICovidDataRepository covidDataRepository;
        private IConfiguration configuration;
        public CovidBLService(ICovidDataRepository covidDataRepository, IConfiguration configuration)
        {
            this.covidDataRepository = covidDataRepository;
            this.path = configuration.GetSection("AppSettings")["Data_LocalCopy"];
        }
        public List<CovidGlobalCaseCountDADto> FetchGlobalCasesFromAPI(Metrics metrics, Locations location)
        {            
            WebRequest request = WebRequest.Create(configuration.GetSection("AppSettings")["covidConfirmedCasesAPI"]);
            if (metrics == Metrics.DEATHS)
            {
                request = WebRequest.Create(configuration.GetSection("AppSettings")["covidDeathsAPI"]);
            }
            else if (metrics == Metrics.RECOVERIES)
            {
                request = WebRequest.Create(configuration.GetSection("AppSettings")["covidRecoveriesAPI"]);
            }
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader readerStream = new StreamReader(dataStream);
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            string responseFromServer = readerStream.ReadToEnd();
            //Console.WriteLine(responseFromServer);
            response.Close();

            TextReader reader = new StringReader(responseFromServer);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            dynamic records = csvReader.GetRecords<dynamic>();
            List<CovidGlobalCaseCountBLDto> bALCaseCountRecords = new List<CovidGlobalCaseCountBLDto>();
            Date latestDate = covidDataRepository.GetLastUpdateDate(metrics.ToDALMetrics(), location.ToDALocations()).Date;
            //IDictionary<string, object> caseCounts = new IDictionary;
            string csvRecordCompositeKey = "";            
            int counterKeys = 0;
            foreach (var rec in records)
            {
                counterKeys = 0;
                csvRecordCompositeKey = "";                
                foreach (KeyValuePair<string, object> count in rec)
                {                    
                    if (count.Key == "Province/State" || count.Key == "Country/Region" || count.Key == "Lat" || count.Key == "Long")
                    {
                        csvRecordCompositeKey += (string)count.Value == "" ?  "Null" + "_" : count.Value + "_";  
                    }                    
                    if (counterKeys > 3)
                    {
                        if (DateTime.Compare(DateTime.ParseExact(count.Key, "M/d/yy", System.Globalization.CultureInfo.InvariantCulture), latestDate) > 0)
                        {
                            CovidGlobalCaseCountBLDto caseRecord = new CovidGlobalCaseCountBLDto();
                            caseRecord.Date = DateTime.ParseExact(count.Key, "M/d/yy", System.Globalization.CultureInfo.InvariantCulture);
                            caseRecord.Count = Convert.ToInt32(count.Value);
                            caseRecord.dbCompositeKey = csvRecordCompositeKey.Remove(csvRecordCompositeKey.Length - 1);
                            bALCaseCountRecords.Add(caseRecord);
                        }                       
                    } 
                    counterKeys++;
                }
            }
            var dALCaseCountRecords = Utils.Utilities.MapCaseCountsBLDTOtoDADTO(bALCaseCountRecords);
            return dALCaseCountRecords;        
        }
        public List<CovidUSCaseCountDADto> FetchUSCasesFromAPI(Metrics metrics, Locations locations)
        {
            WebRequest request = WebRequest.Create(configuration.GetSection("AppSettings")["covidConfirmedCases_USAPI"]);
            if (metrics == Metrics.DEATHS && locations == Locations.US)
            {
                request = WebRequest.Create(configuration.GetSection("AppSettings")["covidDeaths_USAPI"]);
            }            
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader readerStream = new StreamReader(dataStream);
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            string responseFromServer = readerStream.ReadToEnd();
            response.Close();
            TextReader reader = new StringReader(responseFromServer);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            dynamic records = csvReader.GetRecords<dynamic>();
            List<CovidUSCaseCountBLDto> bLLCaseCountRecords = new List<CovidUSCaseCountBLDto>();
           Date latestDate = covidDataRepository.GetLastUpdateDate(metrics.ToDALMetrics(), locations.ToDALocations()).Date;
            string combinedKeyHolder = "";
            foreach (var rec in records)
            {                
                foreach (var count in rec)
                {
                    CovidUSCaseCountBLDto caseRecord = new CovidUSCaseCountBLDto();
                    if (count.Key != "UID" && count.Key != "iso2" && count.Key != "iso3" && count.Key != "code3"
                            && count.Key != "FIPS" && count.Key != "Admin2" && count.Key != "Province_State" && count.Key != "Country_Region"
                            && count.Key != "Lat" && count.Key != "Long_" && count.Key == "Combined_Key")
                    {
                        caseRecord.Combined_Key = count.Value;
                        if(caseRecord.Combined_Key != null)
                        {
                            combinedKeyHolder = caseRecord.Combined_Key;
                        }
                    }
                    else if (count.Key != "UID" && count.Key != "iso2" && count.Key != "iso3" && count.Key != "code3"
                            && count.Key != "FIPS" && count.Key != "Admin2" && count.Key != "Province_State" && count.Key != "Country_Region"
                            && count.Key != "Lat" && count.Key != "Long_" && count.Key != "Combined_Key" && count.Key != "Population" 
                            && DateTime.Compare(DateTime.ParseExact(count.Key, "M/d/yy", System.Globalization.CultureInfo.InvariantCulture), latestDate) > 0)
                    {
                        if(combinedKeyHolder != null)
                        {
                            caseRecord.Combined_Key = combinedKeyHolder;
                            caseRecord.Date = DateTime.ParseExact(count.Key, "M/d/yy", System.Globalization.CultureInfo.InvariantCulture);
                            caseRecord.Count = Convert.ToInt32(count.Value);
                            bLLCaseCountRecords.Add(caseRecord);
                        }                       
                    } 
                }                
            }
            var dALCaseCountRecords = Utilities.MapUSCaseCountsBLDTOtoDADTO(bLLCaseCountRecords);
            return dALCaseCountRecords;
        }
        public List<CovidGlobalCaseCountDADto> FetchCasesFromFile(Metrics metrics, Locations locations)
        {
            //Needs fixing 
            TextReader reader = new StreamReader(path);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            dynamic records = csvReader.GetRecords<dynamic>();
            var bALRecords = FetchLocationsFromFile().Item1;
            var headerRow = FetchLocationsFromFile().Item2;
            List<CovidGlobalCaseCountBLDto> bLLCaseCountRecords = new List<CovidGlobalCaseCountBLDto>();
            int id = 1;
            Date latestDate = covidDataRepository.GetLastUpdateDate(metrics.ToDALMetrics(), locations.ToDALocations()).Date;
            foreach (var rec in records)
            {
                IDictionary<string, object> caseCounts = rec;
                foreach (var cases in caseCounts)
                {
                    if (cases.Key != "State" && cases.Key != "Country" && cases.Key != "Lat" && cases.Key != "Long" && DateTime.Compare((DateTime.ParseExact(cases.Key, "M/d/yy", System.Globalization.CultureInfo.InvariantCulture)), latestDate) > 0)
                    {
                        CovidGlobalCaseCountBLDto caseRecord = new CovidGlobalCaseCountBLDto
                        {
                            dbCompositeKey = "Change this",
                            Date = DateTime.ParseExact(cases.Key, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Count = Convert.ToInt32(cases.Value)
                        };
                        bLLCaseCountRecords.Add(caseRecord);
                    }
                }
                id++;
            }
            var dALCaseCountRecords = Utilities.MapCaseCountsBLDTOtoDADTO(bLLCaseCountRecords);
            return dALCaseCountRecords;
        }
        public (List<CovidLocationDADto>, string[]) FetchLocationsFromFile()
        {
            TextReader reader = new StreamReader(path);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var csvRecords = csvReader.GetRecords<CovidLocationBLDto>();
            csvReader.Read();
            csvReader.ReadHeader();
            string[] headerRow = csvReader.Context.HeaderRecord;
            List<CovidLocationBLDto> bALRecords = new List<CovidLocationBLDto>();
            foreach (var rec in csvRecords)
            {
                bALRecords.Add(rec);
            }
            var dALRecords = Utils.Utilities.MapLocationsBLDTOtoDADTO(bALRecords);
            return (dALRecords, headerRow);
        }
        public List<CovidLocations_USDADto> FetchUSLocationsFromAPI()
        {
            WebRequest request = WebRequest.Create(configuration.GetSection("AppSettings")["covidConfirmedCases_USAPI"]);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader readerStream = new StreamReader(dataStream);
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            string responseFromServer = readerStream.ReadToEnd();
            response.Close();
            TextReader reader = new StringReader(responseFromServer);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            dynamic records = csvReader.GetRecords<dynamic>();
            List<CovidLocations_USBLDto> bALRecords = new List<CovidLocations_USBLDto>();
            foreach (var rec in records)
            {
                CovidLocations_USBLDto location = new CovidLocations_USBLDto();
                foreach (KeyValuePair<string, object> locationInRecord in rec)
                {
                    if (locationInRecord.Key == "Admin2")
                    {
                        location.County = (string)locationInRecord.Value;
                    }
                    if (locationInRecord.Key == "Province_State")
                    {
                        location.State = (string)locationInRecord.Value;
                    }
                    if (locationInRecord.Key == "Country_Region")
                    {
                        location.Country = (string)locationInRecord.Value;
                    }
                    if (locationInRecord.Key == "Combined_Key")
                    {
                        location.Combined_Key = (string)locationInRecord.Value;
                    }
                    if (locationInRecord.Key == "Lat")
                    {
                        location.Lat = Convert.ToDouble(locationInRecord.Value);
                    }
                    if (locationInRecord.Key == "Long_")
                    {
                        location.Long = Convert.ToDouble(locationInRecord.Value);
                    }
                }
                bALRecords.Add(location);
            }
            var dALRecords = Utils.Utilities.MapUSLocationsBLDTOtoDADTO(bALRecords);
            return dALRecords;
        }
        public void InsertLocations(Locations? locations)
        {
            if (locations == Locations.US)
            {
                covidDataRepository.InsertUSLocations(FetchUSLocationsFromAPI());
            }
            else if(locations == Locations.Global)
            {
                covidDataRepository.InsertGlobalLocations(FetchLocationsFromFile().Item1);
            }
        }
        public void InsertCases(Metrics metrics, Locations locations)
        {
            if(locations == Locations.US)
            {                
                covidDataRepository.InsertUSCases(FetchUSCasesFromAPI(metrics, locations), metrics.ToDALMetrics());
            }
            else if (locations == Locations.Global)
            {
                covidDataRepository.InsertGlobalCases(FetchGlobalCasesFromAPI(metrics, locations), metrics.ToDALMetrics());
            }            
        }
        public List<NationalCasesBLDto> GetCountOfCasesForAllNations(Metrics metrics)
        {            
            return Utils.Utilities.MapNationalCasesDADTOtoBLDTO(covidDataRepository.GetCountOfCasesForAllNations(metrics.ToDALMetrics()));
        }        
        public List<NationalCasesBLDto> GetCountOfCasesByCountry(Metrics metrics, string Country, DateTime? Date)
        {
            return Utils.Utilities.MapNationalCasesDADTOtoBLDTO(covidDataRepository.GetCountOfCasesByCountry(metrics.ToDALMetrics(), Country, Date));
        }
        public List<GlobalTotalCountsBLDto> GetGlobalTotalCounts(Metrics metrics)
        {
            return Utils.Utilities.MapGlobalTotalCountsDADTOtoBLDTO(covidDataRepository.GetGlobalTotalCounts(metrics.ToDALMetrics()));
        }
        public List<DailyCaseCountsBLDto> GetDailyCaseCountsByCountry(Metrics metrics, string Country)
        {
            return Utils.Utilities.MapDailyCasesDADTOtoBLDTO(covidDataRepository.GetDailyCaseCountsByCountry(metrics.ToDALMetrics(), Country));
        }
        public List<USCountyCaseCountBLDto> GetCountOfUSCasesByCounty(Metrics metrics, Locations locations, string State, string County, DateTime? Date)
        {
            return Utils.Utilities.MapUSCountyCasesDADtoToBLDto(covidDataRepository.GetCountOfUSCasesByCounty(metrics.ToDALMetrics(), locations.ToDALocations(), State, County, Date));
        }
        public List<USStateCaseCountBLDto> GetCountOfUSCasesByState(Metrics metrics, Locations locations, string State, DateTime? Date)
        {
            return Utilities.MapUSStateCasesDADtoToBLDto(covidDataRepository.GetCountOfUSCasesByState(metrics.ToDALMetrics(), locations.ToDALocations(), State, Date));
        }
        public List<LocationNameBLDto> GetListOfLocations(Locations locations, string region)
        {
            return Utilities.MapLocationNamesDADTOtoBLDTO(covidDataRepository.GetListOfLocations(locations.ToDALocations(), region));
        }

    }
}
