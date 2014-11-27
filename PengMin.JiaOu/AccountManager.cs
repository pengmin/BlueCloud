using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PengMin.JiaOu.SysConfig;

namespace PengMin.JiaOu
{
	public partial class AccountManager : Form
	{
		private readonly SystemConfig _config;

		public AccountManager()
		{
			_config = new SysConfigManager().Get();
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			dataGridView1.Rows.Clear();
			foreach (var item in _config.Accounts)
			{
				dataGridView1.Rows.Add(false, item.Name, item.Server, item.User, item.Password, item.Database);
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = item;
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			var info = new AccountInfo();
			var accForm = new Account(info);
			if (accForm.ShowDialog() != DialogResult.OK) return;

			_config.AddAccountInfo(info);
			new SysConfigManager().Set(_config);

			Init();
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.Cells[0].EditedFormattedValue.ToString() == true.ToString())
				{
					_config.RemoveAccountInfo((AccountInfo)row.Tag);
				}
			}
			new SysConfigManager().Set(_config);
			Init();
		}
	}
}
