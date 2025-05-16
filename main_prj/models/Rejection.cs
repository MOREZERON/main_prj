using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main_prj.models
{
    public class Rejection
    {
        public int RejectionID { get; set; }
        public string ProductName { get; set; }
        public string DefectName { get; set; }
        public int Quantity { get; set; }
        public string Comment { get; set; }
    }
}
