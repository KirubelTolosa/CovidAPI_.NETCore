using Covid.BLL.Service.Models;
using Covid.DAL.Service.Models;


namespace Covid.BLL.Service.Utils
{
    public static partial class Extensions
    {
        public static LocationNameBLDto ToLocationNamesBLDTO(this LocationNameDADto rec)
        {
            return new LocationNameBLDto
            {
                location = rec.location
            };
        }
        public static CovidGlobalCaseCountDADto ToCovidGlobalCaseCoutDADto(this CovidGlobalCaseCountBLDto rec)
        {
            return new CovidGlobalCaseCountDADto
            {
                dbCompositeKey = rec.dbCompositeKey,
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static CovidCaseCountDADto ToCaseCountstDADto(this CovidGlobalCaseCountBLDto rec)
        {
            return new CovidCaseCountDADto
            {
                dbCompositeKey = rec.dbCompositeKey, 
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static CovidUSCaseCountDADto ToUSCaseCountstDADto(this CovidUSCaseCountBLDto rec)
        {
            return new CovidUSCaseCountDADto
            {
                Combined_Key = rec.Combined_Key,
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static CovidLocationDADto ToLocationsDADto(this CovidLocationBLDto rec)
        {
            return new CovidLocationDADto
            {
                Country = rec.Country,
                State = rec.State,
                Lat = rec.Lat,
                Long = rec.Long
            };
        }
        public static CovidLocations_USDADto ToUSLocationsDADto(this CovidLocations_USBLDto rec)
        {
            return new CovidLocations_USDADto
            {
                County = rec.County,
                State = rec.State,
                Country = rec.Country,
                Combined_Key = rec.Combined_Key,
                Lat = rec.Lat,
                Long = rec.Long              
            };
        }
        public static CovidLocationBLDto ToLocationsBLDto(this CovidLocationDADto rec)
        {
            return new CovidLocationBLDto
            {
                Country = rec.Country,
                State = rec.State,
                Lat = rec.Lat,
                Long = rec.Long
            };
        }        
        public static CovidGlobalCaseCountBLDto ToCaseCountsBLDto(this CovidCaseCountDADto rec)
        {
            return new CovidGlobalCaseCountBLDto
            {
                dbCompositeKey = rec.dbCompositeKey,                
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static DailyCaseCountsBLDto ToDailyCasesBLDto(this DailyCaseCountsDADto rec)
        {
            return new DailyCaseCountsBLDto
            {
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static NationalCasesBLDto ToNationalCasesBLDto(this NationalCasesDADto rec)
        {
            return new NationalCasesBLDto
            {
                Country = rec.Country,                
                Count = rec.Count
            };
        }

        public static USCountyCaseCountBLDto ToUSCountyCasesBLDto(this USCountyCaseCountDADto rec)
        {
            return new USCountyCaseCountBLDto
            {
                County = rec.County,
                State = rec.State,
                Date = rec.Date,
                Count = rec.Count
            };
        }
        
        public static USStateCaseCountBLDto ToUSStateCasesBLDto(this USStateCaseCountDADto rec)
        {
            return new USStateCaseCountBLDto
            {               
                State = rec.State,
                Date = rec.Date,
                Count = rec.Count
            };
        }
        public static GlobalTotalCountsBLDto ToGlobalCountsBLDto(this GlobalCaseCountsDADto rec)
        {
            return new GlobalTotalCountsBLDto
            {
                Count = rec.Count
            };
        }


    }
}
