using Covid.BLL.Service.Models;
using Covid.DAL.Service.Models;
using System.Collections.Generic;

namespace Covid.BLL.Service.Utils
{
    public class Utilities
    {
        public static List<LocationNameBLDto> MapLocationNamesDADTOtoBLDTO(List<LocationNameDADto> dADtos)
        {
            List<LocationNameBLDto> bLDtos = new List<LocationNameBLDto>();
            foreach(var rec in dADtos)
            {
                bLDtos.Add(rec.ToLocationNamesBLDTO());
            }
            return bLDtos;
        }
        public static List<CovidGlobalCaseCountDADto> MapCaseCountsBLDTOtoDADTO(List<CovidGlobalCaseCountBLDto> bLRecords)
        {
            List<CovidGlobalCaseCountDADto> dALRecords = new List<CovidGlobalCaseCountDADto>();
            foreach (var rec in bLRecords)
            {
                dALRecords.Add(rec.ToCovidGlobalCaseCoutDADto());
            }
            return dALRecords;
        }
        public static List<CovidUSCaseCountDADto> MapUSCaseCountsBLDTOtoDADTO(List<CovidUSCaseCountBLDto> bLRecords)
        {
            List<CovidUSCaseCountDADto> dALRecords = new List<CovidUSCaseCountDADto>();
            foreach (var rec in bLRecords)
            {
                dALRecords.Add(rec.ToUSCaseCountstDADto());
            }
            return dALRecords;
        }
        public static List<CovidLocationDADto> MapLocationsBLDTOtoDADTO(List<CovidLocationBLDto> bLRecords)
        {
            List<CovidLocationDADto> dALRecords = new List<CovidLocationDADto>();
            foreach (var rec in bLRecords)
            {
                dALRecords.Add(rec.ToLocationsDADto());
            }
            return dALRecords;
        }
        public static List<CovidLocations_USDADto> MapUSLocationsBLDTOtoDADTO(List<CovidLocations_USBLDto> bLRecords)
        {
            List<CovidLocations_USDADto> dALRecords = new List<CovidLocations_USDADto>();
            foreach (var rec in bLRecords)
            {
                dALRecords.Add(rec.ToUSLocationsDADto());
            }
            return dALRecords;
        }
        public static List<CovidLocationBLDto> MapLocationsDADTOtoBLDTO(List<CovidLocationDADto> bLRecords)
        {
            List<CovidLocationBLDto> dALRecords = new List<CovidLocationBLDto>();
            foreach (var rec in bLRecords)
            {
                dALRecords.Add(rec.ToLocationsBLDto());
            }
            return dALRecords;
        }
        public static List<CovidGlobalCaseCountBLDto> MapCasecountsDADTotoBLDto(List<CovidCaseCountDADto> bLRecords)
        {
            List<CovidGlobalCaseCountBLDto> bLLRecords = new List<CovidGlobalCaseCountBLDto>();
            foreach (var rec in bLRecords)
            {
                bLLRecords.Add(rec.ToCaseCountsBLDto());
            }
            return bLLRecords;
        }
        public static List<USCountyCaseCountBLDto> MapUSCountyCasesDADtoToBLDto(List<USCountyCaseCountDADto> bLRecords)
        {
            List<USCountyCaseCountBLDto> bLLRecords = new List<USCountyCaseCountBLDto>();
            foreach (var rec in bLRecords)
            {
                bLLRecords.Add(rec.ToUSCountyCasesBLDto());
            }
            return bLLRecords;
        }
        public static List<USStateCaseCountBLDto> MapUSStateCasesDADtoToBLDto(List<USStateCaseCountDADto> bLRecords)
        {
            List<USStateCaseCountBLDto> bLLRecords = new List<USStateCaseCountBLDto>();
            foreach (var rec in bLRecords)
            {
                bLLRecords.Add(rec.ToUSStateCasesBLDto());
            }
            return bLLRecords;
        }
        
        public static List<NationalCasesBLDto> MapNationalCasesDADTOtoBLDTO(List<NationalCasesDADto> dALRecords)
        {
            List<NationalCasesBLDto> bLLRecords = new List<NationalCasesBLDto>();
            foreach (var rec in dALRecords)
            {
                bLLRecords.Add(rec.ToNationalCasesBLDto());
            }
            return bLLRecords;
        }
        public static List<DailyCaseCountsBLDto> MapDailyCasesDADTOtoBLDTO(List<DailyCaseCountsDADto> dALRecords)
        {            
            List<DailyCaseCountsBLDto> bLLRecords = new List<DailyCaseCountsBLDto>();
            foreach (var rec in dALRecords)
            {                
                bLLRecords.Add(rec.ToDailyCasesBLDto());
            }
            return bLLRecords;
        }

        public static List<GlobalTotalCountsBLDto> MapGlobalTotalCountsDADTOtoBLDTO(List<GlobalTotalCountsDADto> dALRecords)
        {
            List<GlobalTotalCountsBLDto> bLLRecords = new List<GlobalTotalCountsBLDto>();
            foreach (var rec in dALRecords)
            {
                //Extension method not working - Review 
                GlobalTotalCountsBLDto tempRec = new GlobalTotalCountsBLDto
                {
                    Count = rec.Count
                };
                bLLRecords.Add(tempRec);
            }
            return bLLRecords;
        }
    }
}
