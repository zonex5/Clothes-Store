using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClothesStore
{
    public partial class MainForm : RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Properties.Settings.Default.filename))
                File.WriteAllBytes(Properties.Settings.Default.filename, Properties.Resources.parts);

            SQL.ResetConnection();
            LoadDepartment();
        }

        private void LoadDepartment()
        {
            tree.Nodes.Clear();
            // заполняем список с отделами
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'departments';", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                string id = record["id"].ToString();
                string name = record["name"].ToString();
                TreeNode node = new TreeNode(name, 0, 0);
                node.Tag = id;
                tree.Nodes.Add(node);
            }
        }

        private void LoadProduct()
        {
            grid.Rows.Clear();
            SQLiteCommand command = new SQLiteCommand(string.Format("SELECT * FROM 'products' WHERE departmen={0};", tree.SelectedNode.Tag), SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                string id = record["id"].ToString();
                string name = record["name"].ToString();
                string description = record["description"].ToString();
                string count1 = record["count1"].ToString();
                string count2 = record["count2"].ToString();
                string price = record["price"].ToString();
                string departmen = record["departmen"].ToString();

                float total = (float)(Convert.ToDouble(price) * Convert.ToInt32(count2));

                grid.Rows.Add(new string[] { id, name, count1, count2, price, total.ToString(), departmen });
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label1.Text = "";

            // нет выбраного узла
            if (e.Node != null)
                rbPanel2.Enabled = true;
            else return;
            LoadProduct();
        }

        private void ribbonButton1_Click(object sender, EventArgs e)    // добавить отдел
        {
            EditorForm form = new EditorForm("Новый отдел");
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SQL.ExecSQL("INSERT INTO 'departments' ('name') VALUES ('" + form.Value + "');");
                //LoadDepartments();
            }
        }

        private void ribbonButton2_Click(object sender, EventArgs e)    // удалить отдел
        {
            if (MessageBox.Show("Удалить выбранный отдел?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                SQL.ExecSQL("DELETE FROM 'departments' WHERE id=" + tree.SelectedNode.Tag.ToString() + "; DELETE FROM 'workplaces' WHERE department=" + tree.SelectedNode.Tag.ToString() + ";");
                //LoadDepartments();
            }
        }

        private void ribbonButton3_Click(object sender, EventArgs e)    // добавить товар
        {
            if (tree.SelectedNode == null) return;

            ProductForm form = new ProductForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                string sDate = String.Format("{0:yyyy-MM-dd}", form.Date);
                SQL.ExecSQL(String.Format("INSERT INTO 'products' ('name','description','count1','price','departmen','worker','invoice','data','count2') VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{2}');", form.Nume, form.Description, form.Count1, form.Price, tree.SelectedNode.Tag, form.Worker, form.Invoice, sDate));
                LoadProduct();
            }
        }

        private void ribbonButton4_Click(object sender, EventArgs e)    // удалить товар
        {
            if (grid.CurrentRow == null) return;

            if (MessageBox.Show("Удалить выбранный товар?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string cmd = string.Format("DELETE FROM 'products' WHERE id={0};", grid.CurrentRow.Cells["id"].Value);
                SQL.ExecSQL(cmd);
                LoadProduct();
            }
        }

        private void ribbonOrbMenuItem2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.filename = openFileDialog1.FileName;
                Properties.Settings.Default.Save();

                //this.Text = "Computer Parts & Soft - " + Path.GetFileName(Properties.Settings.Default.filename);
                this.Text = "Учёт офисной техники - " + Properties.Settings.Default.filename;
                SQL.ResetConnection();
                //LoadDepartments();
            }
        }

        private void ribbonOrbMenuItem3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, Properties.Resources.parts);

                this.Text = "Учёт офисной техники - " + Properties.Settings.Default.filename;
                SQL.ResetConnection();
                //LoadDepartments();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

        }

        private void ribbon1_OrbClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label1.Text = "";
            if (grid.CurrentRow == null) return;

            SQLiteCommand command = new SQLiteCommand(string.Format("SELECT description, workers.name as worker, invoice, datetime(data) as data  FROM 'products', 'workers' WHERE products.id={0} AND workers.id=products.worker;", grid.CurrentRow.Cells["id"].Value), SQL.Connection);

            foreach (DbDataRecord record in command.ExecuteReader())
            {
                try { label5.Text = Convert.ToDateTime(record["data"]).ToLongDateString(); } catch { label5.Text = "неизвестно"; }
                label6.Text = record["invoice"].ToString();
                label7.Text = record["worker"].ToString();
                label1.Text = record["description"].ToString();
            }
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            new Workers().ShowDialog();
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            new SellForm().ShowDialog();
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            new HistoryForm().ShowDialog();
        }

        private void ribbonButton9_Click(object sender, EventArgs e)
        {
            new SellsForm().ShowDialog();
        }

        private void ribbonButton10_Click(object sender, EventArgs e)
        {
            new WorkerForm().ShowDialog();
        }
    }
}
