using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using PengMin.Infrastructure;
using PengMin.JiaOu.Dal;
using PengMin.JiaOu.SysConfig;

namespace PengMin.JiaOu
{
	public partial class MainForm : Form
	{
		private AccountInfo _from, _to;

		public MainForm()
		{
			InitializeComponent();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			new AccountManager().ShowDialog();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() == DialogResult.OK)
			{
				_from = sl.CheckedInfo;
				var data = new DataAccess(new SqlHelper(_from.GetConnectionString())).GetPurchaseOrder();
				dataGridView1.Columns.Clear();
				foreach (DataColumn cln in data.Columns)
				{
					dataGridView1.Columns.Add(cln.ColumnName, cln.ColumnName);
				}
				dataGridView1.Columns[0].Visible = false;
				foreach (DataRow row in data.Rows)
				{
					dataGridView1.Rows.Add(
						row["id"],
						row["单据日期"],
						row["单据编号"],
						row["供应商"],
						row["业务员"],
						row["预计到货日期"],
						row["付款方式"],
						row["订金金额"],
						row["预付款百分比"]
					);
				}
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() == DialogResult.OK)
			{
				var sh = new SqlHelper(_to.GetConnectionString());
				_to = sl.CheckedInfo;
				var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
				var access = new DataAccess(sh);
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					DataTable main, detail;
					access.GetPurchaseOrder((Guid)row.Cells[0].Value, out main, out detail);
					sqlList.AddRange(access.PurchaseOrderToSaleOrder(main, detail));
				}
				if (sqlList.Count > 0)
				{
					sh.Open();
					sh.Execute(sqlList);
					sh.Close();
				}
			}
		}
	}
}
