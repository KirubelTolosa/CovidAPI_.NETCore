using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid.BLL.Service.Models
{
    public class USCountyCaseCountBLDto
    {
        public string County { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
