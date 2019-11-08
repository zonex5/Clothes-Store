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
    public partial class ProductForm : Form
    {
        public string Nume { get; set; }
        public string Description { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public float Price { get; set; }
        public string Invoice { get; set; }
        public string Worker { get; set; }
        public DateTime Date { get; set; }

        public string ID { get; set; }

        public ProductForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Название товара не может быть пустым.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            if (textBox6.Text.Length == 0)
            {
                MessageBox.Show("Необходимо указать накладную.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("Необходимо указать количество товара.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            if (textBox5.Text.Length == 0)
            {
                MessageBox.Show("Необходимо указать цену.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                Nume = textBox1.Text;
                Description = textBox2.Text;
                try { Count1 = Convert.ToInt32(textBox3.Text); } catch { Count1 = 0; }
                try { Price = Convert.ToInt32(textBox5.Text); } catch { Price = 0; }
                Date = dateTimePicker1.Value;
                Worker = (comboBox1.SelectedItem as Item).id;
                Invoice = textBox6.Text;

                DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }  // cancel

        private void StationForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            // заполняем список работников
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'workers';", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                Item item = new Item();
                item.id = record["id"].ToString();
                item.name = record["name"].ToString();

                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }
    }
}
