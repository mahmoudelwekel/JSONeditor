using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace JSONeditor
{
    public partial class JSONeditor : Form
    {
 
        public JSONeditor()
        {
            InitializeComponent();
            AttachDatabase();
        }

        void AttachDatabase()
        {
            try
            {
                openFileDialog1.Filter = "Json files (*.json)|*.json";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                { 

                    DatabaseTextBox.Text = openFileDialog1.FileName;

                    var table = JsonConvert.DeserializeObject<DataTable>(File.ReadAllText(DatabaseTextBox.Text));
                    dataGridView1.DataSource = table;

                }
            }
            catch (Exception errortext)
            {
                MessageBox.Show(errortext.ToString());
            }
        }

      

        private void Attach_Click(object sender, EventArgs e)
        {
            try
            {
                AttachDatabase();
            }
            catch (Exception errortext)
            {
                MessageBox.Show(errortext.ToString());
            }
        }

        private void ExtractBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string output = JsonConvert.SerializeObject(this.dataGridView1.DataSource,Formatting.Indented);

                System.IO.File.WriteAllText(DatabaseTextBox.Text, output);

                MessageBox.Show("Saved successfully!", DateTime.Now.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception errortext)
            {
                MessageBox.Show(errortext.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //var bd = (BindingSource)dataGridView1.DataSource;
                var dt = (DataTable)dataGridView1.DataSource;

                List<string> QueryList = new List<string>();

                foreach (DataColumn item in dt.Columns)
                {
                    string Query = string.Format(item.ColumnName+" like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
                    QueryList.Add(Query);
                }


                dt.DefaultView.RowFilter = string.Join(" OR ", QueryList); 
                dataGridView1.Refresh();
            }
            catch (Exception errortext)
            {
                MessageBox.Show(errortext.ToString());
            }
        }
    }
}
