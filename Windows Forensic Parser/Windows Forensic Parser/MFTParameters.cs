using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_Parser
{
    public class MFTParameters
    {
        public string Starting_Signature { get; set; }
        public int Offset_to_Fixup_Array { get; set; }
        public int Number_of_Entries_in_Fixup_Array { get; set; }
        public long LogFile_Sequence_Number { get; set; }
        public int Sequence_Number { get; set; }
        public int Hard_Link_Count { get; set; }
        public int Offset_to_First_Attribute { get; set; }
        public string Attribute_Flags { get; set; }
        public int Real_Size_of_MFT_Record { get; set; }
        public int Allocated_Size_of_MFT_Record { get; set; }
        public long File_Reference_to_the_Base_MFT_Record { get; set; }
        public int Next_Attribute_Id { get; set; }
        public int MFT_Record_Number { get; set; }
        public DateTime File_Creation_Time_SI { get; set; }
        public DateTime File_Altered_Time_SI { get; set; }
        public DateTime MFT_Altered_Time_SI { get; set; }
        public DateTime File_Accessed_Time_SI { get; set; }
        public long Parent_MFT_File_Record { get; set; }
        public int Parent_Sequence_Number { get; set; }
        public DateTime File_Creation_Time_FN { get; set; }
        public DateTime File_Altered_Time_FN { get; set; }
        public DateTime MFT_Altered_Time_FN { get; set; }
        public DateTime File_Accessed_Time_FN { get; set; }        
        public int File_Name_Length_in_Characters { get; set; }
        public string Namespace { get; set; }
        public string File_Name { get; set; }
        public string Resident_Data { get; set; }
        public long Starting_Virtual_Cluster_Number { get; set; }
        public long Ending_Virtual_Cluster_Number { get; set; }
        public int Offset_to_Runlist { get; set; }
        public long Allocated_Size_of_Content { get; set; }
        public long Actual_Size_of_Content { get; set; }
        public long Initialised_Size_of_Content { get; set; }        
    }

    public class MFTAttributeHeaderEntries
    {
        public string AttributeIdentifier { get; set; }
        public int AttributeSize { get; set; }
        public string NonResidentFlag { get; set; }
        public int AttributeLength { get; set; }
        public int OffsetToAttributeName { get; set; }
        public string Flags { get; set; }
        public int AttributeId { get; set; }
        public int SizeOfContent { get; set; }
        public int OffsetToContent { get; set; }
    }
}


