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
    public partial class SellsForm : Form
    {
        public SellsForm()
        {
            InitializeComponent();
        }

        private void SellsForm_Load(object sender, EventArgs e)
        {
            grid.Rows.Clear();
            SQLiteCommand command = new SQLiteCommand("SELECT history.product, products.name, count(*) as cntsells, sum(history.count) as totalsells, sum(history.totalprice) as price FROM products, history WHERE products.id=history.product GROUP BY history.product;", SQL.Connection);
            foreach (DbDataRecord record in command.ExecuteReader())
            {
                grid.Rows.Add(new object[] { record["name"], record["cntsells"], record["totalsells"], record["price"] });
            }
        }
    }
}
