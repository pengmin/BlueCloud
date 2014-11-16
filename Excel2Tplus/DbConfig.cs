using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus
{
	public partial class DbConfig : Form
	{
		public DbConfig()
		{
			InitializeComponent();
		}

		public void InitConfig(SystemConfig.DatabaseConfig config)
		{
			server.Text = config.Server;
			user.Text = config.UserName;
			password.Text = config.Password;
			database.Text = config.Database;
		}

		public void SetConfig(SystemConfig.DatabaseConfig config)
		{
			config.Server = server.Text;
			config.UserName = user.Text;
			config.Password = password.Text;
			config.Database = database.Text;
		}

		private void ok_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(server.Text) ||
				string.IsNullOrWhiteSpace(user.Text) ||
				string.IsNullOrWhiteSpace(password.Text) ||
				string.IsNullOrWhiteSpace(database.Text))
			{
				MessageBox.Show("请填写完整的数据库配置信息！");
				return;
			}
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
