using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class DirectoryEntryParameters
    {
        public string File_Name { get; set; }
        public string File_Extension { get; set; }
        public string Attribute_Flag { get; set; }
        public string Creation_Time_in_HEX { get; set; }
        public string Last_Accessed_Date_in_HEX { get; set; }
        public string Last_Written_Time_in_HEX { get; set; }
        public int Starting_Cluster { get; set; }
        public int Size_in_Bytes { get; set; }
    }
}
