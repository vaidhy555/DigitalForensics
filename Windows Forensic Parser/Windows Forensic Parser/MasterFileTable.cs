using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automatic_Parser
{
    public class MasterFileTable
    {
        public static DataTable ParseMFT(string path)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                int entries = hex.Length / 1024;
                int loopCount = 1;
                int takeLength = 1024;
                int skipLength = 0;

                List<string[]> mftEntries = new List<string[]>();

                //Logic to split data into 1024 byte chunks.
                while (loopCount <= entries)
                {
                    mftEntries.Add(hex.Skip(skipLength).Take(takeLength).ToArray());
                    skipLength = loopCount * 1024;
                    loopCount++;
                }

                List<MFTParameters> mftDetails = new List<MFTParameters>();

                Parallel.ForEach(mftEntries, (entry) =>
                {
                    MFTParameters mftEntryObj = new MFTParameters();
                    mftEntryObj.Starting_Signature = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Take(4))));
                    mftEntryObj.Offset_to_Fixup_Array = int.Parse(string.Join("", hex.Skip(4).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Number_of_Entries_in_Fixup_Array = int.Parse(string.Join("", hex.Skip(6).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.LogFile_Sequence_Number = long.Parse(string.Join("", hex.Skip(8).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Sequence_Number = int.Parse(string.Join("", hex.Skip(16).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Hard_Link_Count = int.Parse(string.Join("", hex.Skip(18).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Offset_to_First_Attribute = int.Parse(string.Join("", hex.Skip(20).Take(2).Reverse()), NumberStyles.HexNumber);
                    string attributeFlag = string.Join("", hex.Skip(22).Take(2).Reverse());
                    switch (attributeFlag)
                    {
                        case "0000": mftEntryObj.Attribute_Flags = "Record is a file and deleted"; break;
                        case "0001": mftEntryObj.Attribute_Flags = "Record is a file and allocated"; break;
                        case "0002": mftEntryObj.Attribute_Flags = "Record is a directory and deleted"; break;
                        case "0003": mftEntryObj.Attribute_Flags = "Record is a directory and allocated"; break;
                        default: mftEntryObj.Attribute_Flags = attributeFlag + " ;Unknown"; break;
                    }
                    mftEntryObj.Real_Size_of_MFT_Record = int.Parse(string.Join("", hex.Skip(24).Take(4).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Allocated_Size_of_MFT_Record = int.Parse(string.Join("", hex.Skip(28).Take(4).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.File_Reference_to_the_Base_MFT_Record = long.Parse(string.Join("", hex.Skip(32).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Next_Attribute_Id = int.Parse(string.Join("", hex.Skip(40).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.MFT_Record_Number = int.Parse(string.Join("", hex.Skip(44).Take(4).Reverse()), NumberStyles.HexNumber);

                    int attributeCount = 0;
                    int offsetToAttribute = mftEntryObj.Offset_to_First_Attribute;

                    while (attributeCount < mftEntryObj.Next_Attribute_Id)
                    {
                        MFTAttributeHeaderEntries attributeHeader = new MFTAttributeHeaderEntries();
                        attributeHeader.AttributeIdentifier = string.Join("", hex.Skip(offsetToAttribute).Take(4));
                        attributeHeader.AttributeSize = int.Parse(string.Join("", hex.Skip(offsetToAttribute + 4).Take(4).Reverse()), NumberStyles.HexNumber);

                        switch (attributeHeader.AttributeIdentifier)
                        {
                            case "10000000": GetStandardInformationAttributes(attributeHeader, mftEntryObj, hex.Skip(offsetToAttribute).Take(attributeHeader.AttributeSize).ToArray()); break;
                            case "30000000": GetFileNameAttributes(attributeHeader, mftEntryObj, hex.Skip(offsetToAttribute).Take(attributeHeader.AttributeSize).ToArray()); break;
                            case "80000000": GetDataAttributes(attributeHeader, mftEntryObj, hex.Skip(offsetToAttribute).Take(attributeHeader.AttributeSize).ToArray()); break;
                            default: break;
                        }

                        offsetToAttribute += attributeHeader.AttributeSize; 
                        attributeCount++;
                    }
                                        
                    mftDetails.Add(mftEntryObj);
                });
               
                DataTable dataTable = Utility.ToDataTable(mftDetails);
                return dataTable;
            }
            catch (Exception ex)
            {
                string title = "Error";
                MessageBox.Show(ex.Message, title);
                return null;
            }
        }

        public static void GetStandardInformationAttributes(MFTAttributeHeaderEntries attributeHeader, MFTParameters mftEntryObj, string[] data)
        {
            try
            {
                attributeHeader.NonResidentFlag = data.Skip(8).Take(1).ToString() == "00" ? "Resident" : "Non - Resident";
                attributeHeader.SizeOfContent = int.Parse(string.Join("", data.Skip(16).Take(4).Reverse()), NumberStyles.HexNumber);
                attributeHeader.OffsetToContent = int.Parse(string.Join("", data.Skip(20).Take(2).Reverse()), NumberStyles.HexNumber);
                mftEntryObj.File_Creation_Time_SI = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.File_Altered_Time_SI = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 8).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.MFT_Altered_Time_SI = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 16).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.File_Accessed_Time_SI = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 24).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
            }
            catch (Exception)
            {
                string title = "Error";
                MessageBox.Show("Error while parsing SI attribute", title);
            }
        }

        public static void GetFileNameAttributes(MFTAttributeHeaderEntries attributeHeader, MFTParameters mftEntryObj, string[] data)
        {
            try
            {
                attributeHeader.NonResidentFlag = data.Skip(8).Take(1).ToString() == "00" ? "Resident" : "Non - Resident";
                attributeHeader.SizeOfContent = int.Parse(string.Join("", data.Skip(16).Take(4).Reverse()), NumberStyles.HexNumber);
                attributeHeader.OffsetToContent = int.Parse(string.Join("", data.Skip(20).Take(2).Reverse()), NumberStyles.HexNumber);
                mftEntryObj.Parent_MFT_File_Record = long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent).Take(8).Reverse().Skip(2).Take(6)), NumberStyles.HexNumber);
                mftEntryObj.Parent_Sequence_Number = int.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent).Take(8).Reverse().Take(2)), NumberStyles.HexNumber);
                mftEntryObj.File_Creation_Time_FN = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 8).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.File_Altered_Time_FN = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 16).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.MFT_Altered_Time_FN = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 24).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.File_Accessed_Time_FN = DateTime.FromFileTimeUtc(long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 32).Take(8).Reverse()), NumberStyles.AllowHexSpecifier));
                mftEntryObj.File_Name_Length_in_Characters = int.Parse(data.Skip(attributeHeader.OffsetToContent + 64).Take(1).First(), NumberStyles.HexNumber);
                switch (data.Skip(attributeHeader.OffsetToContent + 65).Take(1).First())
                {
                    case "00": mftEntryObj.Namespace = "POSIX"; break;
                    case "01": mftEntryObj.Namespace = "Win32"; break;
                    case "02": mftEntryObj.Namespace = "DOS"; break;
                    case "03": mftEntryObj.Namespace = "Win32 & DOS"; break;
                    default: mftEntryObj.Namespace = "unknown"; break;
                }
                mftEntryObj.File_Name = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", data.Skip(attributeHeader.OffsetToContent + 66).Take(mftEntryObj.File_Name_Length_in_Characters * 2))));
            }
            catch (Exception)
            {
                string title = "Error";
                MessageBox.Show("Error while parsing FN attribute", title);
            }
        }

        public static void GetDataAttributes(MFTAttributeHeaderEntries attributeHeader, MFTParameters mftEntryObj, string[] data)
        {
            try
            {
                attributeHeader.NonResidentFlag = data.Skip(8).Take(1).ToString() == "00" ? "Resident" : "Non - Resident";
                if (attributeHeader.NonResidentFlag == "Resident")
                {
                    attributeHeader.SizeOfContent = int.Parse(string.Join("", data.Skip(16).Take(4).Reverse()), NumberStyles.HexNumber);
                    attributeHeader.OffsetToContent = int.Parse(string.Join("", data.Skip(20).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Resident_Data = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", data.Skip(attributeHeader.OffsetToContent).Take(attributeHeader.SizeOfContent))));
                }
                if (attributeHeader.NonResidentFlag == "Non - Resident")
                {
                    mftEntryObj.Starting_Virtual_Cluster_Number = long.Parse(string.Join("",data.Skip(attributeHeader.OffsetToContent + 16).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Ending_Virtual_Cluster_Number = long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 24).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Offset_to_Runlist = int.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 32).Take(2).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Allocated_Size_of_Content = long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 40).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Actual_Size_of_Content = long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 48).Take(8).Reverse()), NumberStyles.HexNumber);
                    mftEntryObj.Initialised_Size_of_Content = long.Parse(string.Join("", data.Skip(attributeHeader.OffsetToContent + 56).Take(8).Reverse()), NumberStyles.HexNumber);
                }

            }
            catch (Exception)
            {
                string title = "Error";
                MessageBox.Show("Error while parsing Data attribute", title);
            }
        }
    }    
}
