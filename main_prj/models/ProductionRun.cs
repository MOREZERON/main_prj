using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main_prj.models
{
    public class ProductionRun
    {
        public int RunID { get; set; }
        public string ProductName { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ProducedAt { get; set; }
        public int QuantityProduced { get; set; }
        public int QuantityAccepted { get; set; }
        public int QuantityRejected { get; set; }
    }
}
