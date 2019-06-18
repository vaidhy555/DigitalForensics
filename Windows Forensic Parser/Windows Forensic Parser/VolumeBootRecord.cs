using AutoMapper;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Automatic_Parser
{
    public class VolumeBootRecord
    {
        public static DataTable ParseVBRFat12Fat16(string path)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                Mapper.Reset();
                Mapper.Initialize(cfg =>
                    cfg.CreateMap<VolumeBootRecordParameters, VBRFat12Fat16Params>()
                );

                VBRFat12Fat16Params vbrParams = new VBRFat12Fat16Params();
                vbrParams = Mapper.Map<VolumeBootRecordParameters, VBRFat12Fat16Params>(GetCommonVBRParams(hex));

                if (vbrParams != null)
                {
                    vbrParams.Logical_Drive_Number = int.Parse(hex[36], NumberStyles.HexNumber);
                    vbrParams.Serial_Number_Of_Partition = string.Join("", hex.Skip(39).Take(4).Reverse());
                    vbrParams.Volume_Label = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(43).Take(11))));
                    vbrParams.File_System_Type = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(54).Take(8))));

                    DataTable dataTable = Utility.ToDataTableDictionary(vbrParams);
                    return dataTable;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string title = "Error";
                MessageBox.Show(ex.Message, title);
                return null;
            }
        }

        public static DataTable ParseVBRFat32(string path)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                Mapper.Reset();
                Mapper.Initialize(cfg =>
                    cfg.CreateMap<VolumeBootRecordParameters, VBRFat32Params>()
                );

                VBRFat32Params vbrParams = new VBRFat32Params();
                vbrParams = Mapper.Map<VolumeBootRecordParameters, VBRFat32Params>(GetCommonVBRParams(hex));

                if (vbrParams != null)
                {
                    vbrParams.Sectors_Per_FAT = int.Parse(string.Join("", hex.Skip(36).Take(4).Reverse()), NumberStyles.HexNumber);
                    vbrParams.First_Cluster_Of_Root_Directory = int.Parse(string.Join("", hex.Skip(44).Take(4).Reverse()), NumberStyles.HexNumber);
                    vbrParams.Filesystem_Information_Sector_Number = int.Parse(string.Join("", hex.Skip(48).Take(2).Reverse()), NumberStyles.HexNumber);
                    vbrParams.Backup_Boot_Sector_Location = int.Parse(string.Join("", hex.Skip(50).Take(2).Reverse()), NumberStyles.HexNumber);
                    vbrParams.Logical_Drive_Number = int.Parse(hex[64], NumberStyles.HexNumber);
                    vbrParams.Serial_Number_Of_Partition = string.Join("", hex.Skip(67).Take(4).Reverse());
                    vbrParams.Volume_Label = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(71).Take(11))));
                    vbrParams.File_System_Type = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(82).Take(8))));

                    DataTable dataTable = Utility.ToDataTableDictionary(vbrParams);
                    return dataTable;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string title = "Error";
                MessageBox.Show(ex.Message, title);
                return null;
            }
        }

        public static VolumeBootRecordParameters GetCommonVBRParams(string[] hex)
        {
            try
            {

                VolumeBootRecordParameters commonParams = new VolumeBootRecordParameters();

                commonParams.Bootcode_Jump_Instruction = string.Join("", hex.Take(3));
                commonParams.OEM_Name = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", hex.Skip(3).Take(8))));
                commonParams.Bytes_Per_Sector = int.Parse(string.Concat(hex[12], hex[11]), NumberStyles.HexNumber);
                commonParams.Sectors_Per_Cluster = int.Parse(hex[13], NumberStyles.HexNumber);
                commonParams.Reserved_Sectors = int.Parse(string.Concat(hex[15], hex[14]), NumberStyles.HexNumber);
                commonParams.Number_Of_FAT_Copies = int.Parse(string.Concat(hex[16]), NumberStyles.HexNumber);
                commonParams.Root_Directory_Entries = int.Parse(string.Concat(hex[18], hex[17]), NumberStyles.HexNumber);
                commonParams.Total_Number_Of_Sectors_in_16Bit_Value = int.Parse(string.Concat(hex[20], hex[19]), NumberStyles.HexNumber);
                commonParams.Media_Descriptor_Type = hex[21].ToUpper() == "F8" ? "Hard Drive" : hex[21];
                commonParams.Sectors_Per_FAT = int.Parse(string.Concat(hex[23], hex[22]), NumberStyles.HexNumber);
                commonParams.Sector_Per_Track = int.Parse(string.Concat(hex[25], hex[24]), NumberStyles.HexNumber);
                commonParams.Number_Of_ReadWrite_Heads = int.Parse(string.Concat(hex[27], hex[26]), NumberStyles.HexNumber);
                commonParams.Number_Of_Sectors_Before_Start_Of_Partition = int.Parse(string.Join("", hex.Skip(28).Take(4).Reverse()), NumberStyles.HexNumber);
                commonParams.Total_Number_Of_Sectors_in_32Bit_Value = int.Parse(string.Join("", hex.Skip(32).Take(4).Reverse()), NumberStyles.HexNumber);

                return commonParams;

            }
            catch (Exception ex)
            {
                string title = "Error";
                MessageBox.Show(ex.Message, title);
                return null;
            }
        }
    }
}
