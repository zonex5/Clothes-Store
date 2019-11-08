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
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            grid.Rows.Clear();
            SQLiteCommand command = new SQLiteCommand("SELECT datetime(history.data) as data, products.name as product, workers.name as worker, history.count as cnt, history.totalprice as price  FROM 'history', 'products', 'workers' WHERE history.worker = workers.id AND history.product = products.id; ", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                grid.Rows.Add(new object[] { record["product"], record["cnt"], Convert.ToDateTime(record["data"]).ToLongDateString(), record["price"], record["worker"] });
            }
        }
    }
}
