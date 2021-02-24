using Covid.BLL.Service;
using Covid.DAL.Service;
using Covid.Injestion.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covid.Injestion
{
    public class Application
    {
        protected readonly ICovidBLService _covidBLService;
        public Application(ICovidBLService covidBLService)
        {
            _covidBLService = covidBLService;
           //_covidDataRepository = covidDataRepository;
        }
        public void Run()
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
