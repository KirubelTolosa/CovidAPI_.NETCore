﻿namespace Covid.BLL.Service.Models
{
    public class CovidLocations_USBLDto
    {
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Combined_Key { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
