using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class MBRParameters
    {
        public string BootIndicator { get; set; }
        public string StartingCHS { get; set; }
        public string PartitionType { get; set; }
        public string EndingCHS { get; set; }
        public long StartingSector { get; set; }
        public long EndingSector { get; set; }
        public long PartitionSize { get; set; }

    }
}
