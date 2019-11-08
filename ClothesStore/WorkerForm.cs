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
    public partial class WorkerForm : Form
    {
        public WorkerForm()
        {
            InitializeComponent();
        }

        private void WorkerForm_Load(object sender, EventArgs e)
        {
            grid.Rows.Clear();
            SQLiteCommand command = new SQLiteCommand("SELECT history.worker, workers.name, count(*) as cntsells, sum(history.count) as totalsells, sum(history.totalprice) as price FROM workers, history WHERE workers.id=history.worker GROUP BY history.worker;", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                grid.Rows.Add(new object[] { record["name"], record["cntsells"], record["totalsells"], record["price"] });
            }
        }
    }
}
