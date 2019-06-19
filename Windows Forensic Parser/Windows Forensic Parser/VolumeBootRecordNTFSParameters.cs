using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class VolumeBootRecordNTFSParameters
    {
        public string Bootcode_Jump_Instruction { get; set; }
        public string OEM_Name { get; set; }
        public int Bytes_Per_Sector { get; set; }
        public int Sectors_Per_Cluster { get; set; }
        public int Reserved_Sectors { get; set; }       
        public string Media_Descriptor_Type { get; set; }        
        public int Total_Sectors { get; set; }
        public int Logical_Cluster_Number_For_MFT { get; set; }
        public int Logical_Cluster_Number_For_MFT_Mirror { get; set; }
        public string Size_Of_MFT_Record { get; set; }
        public int Clusters_per_Index_Buffer { get; set; }
        public string Volume_Serial_Number { get; set; }
    }
}
