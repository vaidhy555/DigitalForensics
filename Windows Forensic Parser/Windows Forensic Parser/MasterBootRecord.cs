using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace Automatic_Parser
{
    public class MasterBootRecord
    {
        //Logic to parse MBR
        public static DataTable ParseMBR(string path, bool isPartitionEntry)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                List<MBRParameters> mbrParams = new List<MBRParameters>();

                List<string[]> partitions = new List<string[]>();

                if (isPartitionEntry)
                {
                    partitions.Add(hex.Take(16).ToArray());
                    partitions.Add(hex.Skip(16).Take(16).ToArray());
                    partitions.Add(hex.Skip(32).Take(16).ToArray());
                    partitions.Add(hex.Skip(48).Take(16).ToArray());
                }
                else
                {
                    partitions.Add(hex.Skip(446).Take(16).ToArray());
                    partitions.Add(hex.Skip(462).Take(16).ToArray());
                    partitions.Add(hex.Skip(478).Take(16).ToArray());
                    partitions.Add(hex.Skip(494).Take(16).ToArray());
                }

                foreach (var partition in partitions)
                {
                    if (!partition.All(x => x == "00"))
                    {
                        MBRParameters mbrObj = new MBRParameters();

                        if (partition[0] == "00")
                        {
                            mbrObj.Boot_Indicator = "Non bootable";
                        }
                        else
                        {
                            mbrObj.Boot_Indicator = "Bootable";
                        }

                        mbrObj.Starting_CHS = string.Concat(partition[1], partition[2], partition[3]);

                        switch (partition[4])
                        {
                            case "01": mbrObj.Partition_Type = "FAT 12"; break;
                            case "04":
                            case "86": mbrObj.Partition_Type = "FAT 16"; break;
                            case "05":
                            case "0F": mbrObj.Partition_Type = "Extended Partition"; break;
                            case "07":
                            case "87": mbrObj.Partition_Type = "NTFS"; break;
                            case "0B":
                            case "0C":
                            case "8B":
                            case "8C": mbrObj.Partition_Type = "FAT 32"; break;
                            case "12": mbrObj.Partition_Type = "EISA"; break;
                            case "42": mbrObj.Partition_Type = "Dynamic disk volume"; break;
                            case "06":
                            case "0E": mbrObj.Partition_Type = "BIGDOS FAT 16"; break;
                            default: mbrObj.Partition_Type = partition[4] + "; type unknown"; break;
                        }

                        mbrObj.Ending_CHS = string.Concat(partition[5], partition[6], partition[7]);

                        mbrObj.Starting_Sector = int.Parse(string.Concat(partition[11], partition[10], partition[9], partition[8]), NumberStyles.HexNumber);

                        mbrObj.Partition_Size = int.Parse(string.Concat(partition[15], partition[14], partition[13], partition[12]), NumberStyles.HexNumber);

                        if (mbrObj.Partition_Size == 0)
                        {
                            mbrObj.Ending_Sector = 0;
                        }
                        else
                        {
                            mbrObj.Ending_Sector = mbrObj.Partition_Size + mbrObj.Starting_Sector - 1;
                        }

                        mbrParams.Add(mbrObj);
                    }
                }

                DataTable dataTable = Utility.ToDataTable(mbrParams);
                return dataTable;
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
