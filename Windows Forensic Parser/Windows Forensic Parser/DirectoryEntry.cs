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
    public class DirectoryEntry
    {
        //Logic to parse Directory Entry
        public static DataTable ParseDirectoryEntry(string path)
        {
            try
            {
                string[] hex = Utility.ReadBinaryFile(path);

                int entries = hex.Length / 32;
                int loopCount = 1;
                int takeLength = 32;
                int skipLength = 0;


                List<string[]> directoryEntries = new List<string[]>();

                //Logic to split data into 32 byte chunks.
                while (loopCount <= entries)
                {
                    directoryEntries.Add(hex.Skip(skipLength).Take(takeLength).ToArray());
                    skipLength = loopCount * 32;
                    loopCount++;
                }

                List<DirectoryEntryParameters> fileDetails = new List<DirectoryEntryParameters>();


                Parallel.ForEach(directoryEntries, (entry, state) =>
                {
                    DirectoryEntryParameters directoryEntryObj = new DirectoryEntryParameters();

                    if (!entry[11].Equals("0F", StringComparison.InvariantCultureIgnoreCase))
                    {
                        switch (entry[11])
                        {
                            case "01": directoryEntryObj.Attribute_Flag = "Read Only"; break;
                            case "02": directoryEntryObj.Attribute_Flag = "Hidden File"; break;
                            case "04": directoryEntryObj.Attribute_Flag = "System File"; break;
                            case "08": directoryEntryObj.Attribute_Flag = "Volume label"; break;
                            case "0F": directoryEntryObj.Attribute_Flag = "Long File Name"; break;
                            case "10": directoryEntryObj.Attribute_Flag = "Directory"; break;
                            case "20": directoryEntryObj.Attribute_Flag = "Archive"; break;
                            default: directoryEntryObj.Attribute_Flag = entry[11] + " ;Unknown"; break;
                        }

                        directoryEntryObj.File_Name = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", entry.Take(8))));
                        directoryEntryObj.File_Extension = Encoding.ASCII.GetString(Utility.StringToByteArray(string.Join("", entry.Skip(8).Take(3))));
                        directoryEntryObj.Creation_Time_in_HEX = string.Join("", entry.Skip(14).Take(4));
                        directoryEntryObj.Last_Accessed_Date_in_HEX = string.Join("", entry.Skip(18).Take(2));
                        directoryEntryObj.Last_Written_Time_in_HEX = string.Join("", entry.Skip(22).Take(4));
                        directoryEntryObj.Starting_Cluster = int.Parse(string.Concat(entry[21] + entry[20] + entry[27] + entry[26]), NumberStyles.HexNumber);
                        directoryEntryObj.Size_in_Bytes = int.Parse(string.Concat(entry[31] + entry[30] + entry[29] + entry[28]), NumberStyles.HexNumber);
                        fileDetails.Add(directoryEntryObj);
                    }
                });

                DataTable dataTable = Utility.ToDataTable(fileDetails);
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
