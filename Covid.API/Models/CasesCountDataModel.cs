using System;

namespace CovidAPI.Models
{
    public class CasesCountDataModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}