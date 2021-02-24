using System;

namespace Covid.DAL.Service.Models
{ 
    public class CovidGlobalCaseCountDADto
    {
        public string dbCompositeKey { get; set; }        
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }    
}
