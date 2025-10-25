using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ReadWriteCSV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            string defaultPath = @"C:\datasets\BigML_Dataset_5f50a62795a9306aa200003e.csv";
            string filePath = "";

            if (File.Exists(defaultPath))
            {
                filePath = defaultPath;
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    filePath = ofd.FileName;
                else
                    return;
            }

            try
            {
                DataTable dt = new DataTable();

                
                using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath, Encoding.UTF8))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(","); 
                    parser.HasFieldsEnclosedInQuotes = true; 

                    bool isHeader = true;

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (isHeader)
                        {
                            foreach (string header in fields)
                                dt.Columns.Add(header.Trim());
                            isHeader = false;
                        }
                        else
                        {
                           
                            while (fields.Length < dt.Columns.Count)
                            {
                                Array.Resize(ref fields, dt.Columns.Count);
                            }
                            
                            if (fields.Length > dt.Columns.Count)
                            {
                                fields = fields.Take(dt.Columns.Count).ToArray();
                            }

                            dt.Rows.Add(fields);
                        }
                    }
                }

                dataGridView1.DataSource = dt;
                MessageBox.Show($"✅ Đọc file CSV thành công!\nĐường dẫn: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi đọc file CSV:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Chưa có dữ liệu để ghi! Hãy đọc file CSV trước.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.FileName = "output.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd.FileName;
                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                   
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        sw.Write(dataGridView1.Columns[i].HeaderText);
                        if (i < dataGridView1.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();

                   
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                sw.Write(row.Cells[i].Value);
                                if (i < dataGridView1.Columns.Count - 1)
                                    sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }
                }

                MessageBox.Show($"Đã ghi file CSV thành công!\nLưu tại: {filePath}");
            }
        }
    }
}
