using System;

namespace Covid.BLL.Service.Models
{
    public class CovidUSCaseCountBLDto
    {
        public string Combined_Key { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }    
}
