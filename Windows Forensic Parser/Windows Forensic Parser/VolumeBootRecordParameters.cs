using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class VolumeBootRecordParameters
    {
        public string Bootcode_Jump_Instruction { get; set; }
        public string OEM_Name { get; set; }
        public int Bytes_Per_Sector { get; set; }
        public int Sectors_Per_Cluster { get; set; }
        public int Reserved_Sectors { get; set; }
        public int Number_Of_FAT_Copies { get; set; }
        public int Root_Directory_Entries { get; set; }
        public int Total_Number_Of_Sectors_in_16Bit_Value { get; set; }
        public string Media_Descriptor_Type { get; set; }
        public int Sectors_Per_FAT { get; set; }
        public int Sector_Per_Track { get; set; }
        public int Number_Of_ReadWrite_Heads { get; set; }
        public int Number_Of_Sectors_Before_Start_Of_Partition { get; set; }
        public int Total_Number_Of_Sectors_in_32Bit_Value { get; set; }
    }

    public class VBRFat12Fat16Params : VolumeBootRecordParameters
    {
        public int Logical_Drive_Number { get; set; }        
        public string Serial_Number_Of_Partition { get; set; }
        public string Volume_Label { get; set; }
        public string File_System_Type { get; set; }
    }

    public class VBRFat32Params : VolumeBootRecordParameters
    {
        public int First_Cluster_Of_Root_Directory { get; set; }
        public int Filesystem_Information_Sector_Number { get; set; }
        public int Backup_Boot_Sector_Location { get; set; }
        public int Logical_Drive_Number { get; set; }
        public string Serial_Number_Of_Partition { get; set; }
        public string Volume_Label { get; set; }
        public string File_System_Type { get; set; }
    }

}
