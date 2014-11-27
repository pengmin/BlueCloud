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
	public partial class AccountSelectForm : Form
	{
		public AccountInfo CheckedInfo;

		public AccountSelectForm(IEnumerable<AccountInfo> infos)
		{
			InitializeComponent();
			foreach (var info in infos)
			{
				listBox1.Items.Add(info);
			}
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			CheckedInfo = (AccountInfo)listBox1.SelectedItem;
			DialogResult = DialogResult.OK;
		}
	}
}
