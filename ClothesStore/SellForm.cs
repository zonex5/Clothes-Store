using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClothesStore
{
    public partial class SellForm : Form
    {
        public SellForm()
        {
            InitializeComponent();
        }

        private void SellForm_Load(object sender, EventArgs e)
        {
            LoadDepartment();
            LoadWorkers();
        }

        private void LoadDepartment()
        {
            comboBox1.Items.Clear();
            // заполняем список с отделами
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'departments';", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                comboBox1.Items.Add(new Item() { id = record["id"].ToString(), name = record["name"].ToString() });
            }

            comboBox1.SelectedIndex = 0;
        }

        private void LoadProduct()
        {
            comboBox2.Items.Clear();
            SQLiteCommand command = new SQLiteCommand(string.Format("SELECT * FROM 'products' WHERE departmen={0};", (comboBox1.SelectedItem as Item).id), SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                comboBox2.Items.Add(new Item() { id = record["id"].ToString(), name = record["name"].ToString(), tag = record["price"] });
            }
        }

        private void LoadWorkers()
        {
            comboBox3.Items.Clear();
            // заполняем список работников
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'workers';", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                comboBox3.Items.Add(new Item() { id = record["id"].ToString(), name = record["name"].ToString() });
            }
            comboBox3.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Товар не выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
                if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("Не указано количество.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string sDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            SQLiteCommand command = new SQLiteCommand(string.Format(@"INSERT INTO 'history' ('data','product','worker','count','totalprice') VALUES ('{0}','{1}','{2}','{3}','{4}');
                                                                      UPDATE 'products' SET count2=count2 - {3} WHERE id={1};", sDate, (comboBox2.SelectedItem as Item).id, (comboBox3.SelectedItem as Item).id, numericUpDown1.Value, textBox1.Text), SQL.Connection);
            MessageBox.Show(command.CommandText);
            command.ExecuteNonQuery();

            DialogResult = DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            try { textBox1.Text = (numericUpDown1.Value * Convert.ToInt32((comboBox2.SelectedItem as Item).tag)).ToString(); } catch { textBox1.Text = "0"; }
        }
    }
}
