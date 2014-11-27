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
	public partial class Account : Form
	{
		private AccountInfo _info;

		public Account(AccountInfo info)
		{
			_info = info;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_info.Name = textBox1.Text;
			_info.Server = textBox2.Text;
			_info.User = textBox3.Text;
			_info.Password = textBox4.Text;
			_info.Database = textBox5.Text;
			DialogResult = DialogResult.OK;
		}
	}
}
