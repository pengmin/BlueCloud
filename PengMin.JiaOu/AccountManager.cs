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
		public AccountManager()
		{
			InitializeComponent();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			var info = new AccountInfo();
			var accForm = new Account(info);
			if (accForm.ShowDialog() == DialogResult.OK)
			{
				//todo:保存账套信息
			}
		}
	}
}
