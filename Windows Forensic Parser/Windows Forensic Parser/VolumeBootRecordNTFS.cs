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
    public class VolumeBootRecordNTFS
    {
        public static DataTable ParseVBRNTFS(string path)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                VolumeBootRecordNTFSParameters vbrParams = new VolumeBootRecordNTFSParameters();
                vbrParams.Bootcode_Jump_Instruction = string.Join("", hex.Take(3));
                vbrParams.OEM_Name = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(3).Take(8))));
                vbrParams.Bytes_Per_Sector = int.Parse(string.Concat(hex[12], hex[11]), NumberStyles.HexNumber);
                vbrParams.Sectors_Per_Cluster = int.Parse(hex[13], NumberStyles.HexNumber);
                vbrParams.Reserved_Sectors = int.Parse(string.Join("", hex.Skip(14).Take(7).Reverse()), NumberStyles.HexNumber);
                vbrParams.Media_Descriptor_Type = hex[21].ToUpper() == "F8" ? "Hard Drive" : hex[21];
                vbrParams.Total_Sectors = int.Parse(string.Join("", hex.Skip(40).Take(8).Reverse()), NumberStyles.HexNumber);
                vbrParams.Logical_Cluster_Number_For_MFT = int.Parse(string.Join("", hex.Skip(48).Take(8).Reverse()), NumberStyles.HexNumber);
                vbrParams.Logical_Cluster_Number_For_MFT_Mirror = int.Parse(string.Join("", hex.Skip(56).Take(8).Reverse()), NumberStyles.HexNumber);
                if( int.Parse(hex[64],NumberStyles.HexNumber) <= 127)
                {
                    vbrParams.Size_Of_MFT_Record = string.Format("{0} clusters", int.Parse(hex[64], NumberStyles.HexNumber));
                }
                else 
                {
                    vbrParams.Size_Of_MFT_Record = string.Format("{0} bytes",Math.Pow(2,Math.Abs(Convert.ToSByte(hex[64], 16))));
                }
                vbrParams.Clusters_per_Index_Buffer = int.Parse(hex[68], NumberStyles.HexNumber);
                vbrParams.Volume_Serial_Number = string.Join("", hex.Skip(72).Take(8).Reverse());

                DataTable dataTable = Utility.ToDataTableDictionary(vbrParams);
                return dataTable;
            }
            catch(Exception ex)
            {
                string title = "Error";
                MessageBox.Show(ex.Message, title);
                return null;
            }
        }
    }
}
