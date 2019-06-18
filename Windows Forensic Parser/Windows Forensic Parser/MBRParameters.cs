using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class MBRParameters
    {
        public string Boot_Indicator { get; set; }
        public string Starting_CHS { get; set; }
        public string Partition_Type { get; set; }
        public string Ending_CHS { get; set; }
        public long Starting_Sector { get; set; }
        public long Ending_Sector { get; set; }
        public long Partition_Size { get; set; }

    }
}
