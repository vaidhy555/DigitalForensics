using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class DirectoryEntryParameters
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AttributeFlag { get; set; }
        public string CreationTimeinHEX { get; set; }
        public string LastAccessedDateinHEX { get; set; }
        public string LastWrittenTimeinHEX { get; set; }
        public int StartingCluster { get; set; }
        public int SizeinBytes { get; set; }
    }
}
