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
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() == DialogResult.OK)
			{
				_to = sl.CheckedInfo;
			}
		}
	}
}
