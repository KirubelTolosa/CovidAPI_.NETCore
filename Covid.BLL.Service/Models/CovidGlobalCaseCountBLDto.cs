using System;

namespace Covid.BLL.Service.Models
{ 
    public class CovidGlobalCaseCountBLDto
    {
        public string dbCompositeKey { get; set; }        
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }    
}
