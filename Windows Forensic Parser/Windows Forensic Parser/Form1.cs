using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Automatic_Parser
{
    public partial class Form1 : Form
    {
        internal string filePath { get; set; }
        internal string selectedItem { get; set; }

        private const string MBR = "Master Boot Record - Entire file";
        private const string MBRPartition = "Master Boot Record - Partition Entries";
        private const string VBR12 = "Volume Boot Record - FAT12/FAT16";
        private const string VBR32 = "Volume Boot Record - FAT32";
        private const string VBRNtfs = "Volume Boot Record - NTFS";
        private const string Directory = "Directory Table Entries - FAT";
        
        public Form1()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        //Browse and import file to be parsed
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                textBox1.Text = openFileDialog1.FileName.Split('\\').Last();
            }
        }

        //select value to be parsed
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                string message = "Please select a value";
                string title = "Error";
                MessageBox.Show(message, title);
            }
            else if (string.IsNullOrEmpty(filePath))
            {
                string message = "Please select a file";
                string title = "Error";
                MessageBox.Show(message, title);
            }
            else
            {
                selectedItem = comboBox1.Items[comboBox1.SelectedIndex].ToString();                              

                switch (selectedItem)
                {
                    case MBR:
                        dataGridView1.DataSource = MasterBootRecord.ParseMBR(filePath, false);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;

                    case MBRPartition:
                        dataGridView1.DataSource = MasterBootRecord.ParseMBR(filePath, true);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;

                    case VBR12:
                        dataGridView1.DataSource = VolumeBootRecord.ParseVBRFat12Fat16(filePath);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;

                    case VBR32:
                        dataGridView1.DataSource = VolumeBootRecord.ParseVBRFat32(filePath);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;

                    case VBRNtfs:
                        dataGridView1.DataSource = VolumeBootRecordNTFS.ParseVBRNTFS(filePath);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;

                    case Directory:
                        dataGridView1.DataSource = DirectoryEntry.ParseDirectoryEntry(filePath);
                        if (dataGridView1.DataSource != null)
                        {
                            dataGridView1.Visible = true;
                            button3.Visible = true;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                        break;


                    default: break;
                }
            }
        }

        //Export values in data grid to excel
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = selectedItem + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Utility.ExportToExcel(dataGridView1, sfd.FileName); // Here dataGridview1 is your grid view name
                string message = "Export Successful";
                string title = "Success";
                MessageBox.Show(message, title);
            }
        }
    }
}
