namespace CovidAPI.Models
{
    public class USCountyCaseCountDataModel
    {
        public string County { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
    }
}