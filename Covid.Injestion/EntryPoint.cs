using Covid.BLL.Service;
using Covid.DAL.Service;
using Covid.Injestion.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covid.Ingestion
{
    public class EntryPoint
    {
        protected readonly ICovidBLService _covidBLService;
        public IConfiguration _configuration;
        public ICovidDataRepository _covidDataRepository;
        public EntryPoint(ICovidBLService covidBLService, IConfiguration configuration, ICovidDataRepository covidDataRepository)
        {
            _covidBLService = covidBLService;
            _configuration = configuration;
            _covidDataRepository = covidDataRepository;
        }
        public void Run(String[] args)
        {
                //_covidBLService.InsertLocations(Locations.Global);                   
                _covidBLService.InsertCases(Metrics.CONFIRMED_CASES.ToBLLMetrics(), Locations.Global.ToBLLocations());
                _covidBLService.InsertCases(Metrics.DEATHS.ToBLLMetrics(), Locations.Global.ToBLLocations());
                _covidBLService.InsertCases(Metrics.RECOVERIES.ToBLLMetrics(), Locations.Global.ToBLLocations());

                //_covidBLService.InsertLocations(Locations.US);
                _covidBLService.InsertCases(Metrics.CONFIRMED_CASES.ToBLLMetrics(), Locations.US.ToBLLocations());
                _covidBLService.InsertCases(Metrics.DEATHS.ToBLLMetrics(), Locations.US.ToBLLocations());
            }
        }
    }    

