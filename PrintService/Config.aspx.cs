using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintService
{
	public partial class Config : System.Web.UI.Page
	{
		private ConfigHelper _helper
		{
			get
			{
				return ConfigHelper.GetInstance(this.Server.MapPath("~/Config.xml"));
			}
		}

		private void InitSqlServerInfo()
		{
			var sqlServer = this._helper.GetSqlServer();
			this.server.Text = sqlServer.Server;
			this.Database.Text = sqlServer.Database;
			this.account.Text = sqlServer.Account;
			this.pswd.Text = sqlServer.Password;
		}
		private void SyncSqlServerInfo()
		{
			this._helper.SetSqlServer(new
			{
				Server = this.server.Text,
				Database = this.Database.Text,
				Account = this.account.Text,
				Password = this.pswd.Text
			});
			this._helper.Save();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.InitSqlServerInfo();
			}
		}
		protected void save_Click(object sender, EventArgs e)
		{
			this.SyncSqlServerInfo();
		}
	}
}