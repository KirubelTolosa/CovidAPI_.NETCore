using System;

namespace Covid.DAL.Service.Models
{ 
    public class CovidUSCaseCountDADto
    {
        public string Combined_Key { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }    
}
