using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PrintService
{
	public class ConfigHelper
	{
		private string _path = null;
		private XmlDocument _xmlDoc = new XmlDocument();

		private ConfigHelper(string path)
		{
			this._path = path;
			this._xmlDoc.Load(this._path);
		}

		public static ConfigHelper GetInstance(string path)
		{
			return new ConfigHelper(path);
		}

		public string SqlConnectionString()
		{
			var sqlServer = this.GetSqlServer();
			return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", sqlServer.Server, sqlServer.Database, sqlServer.Account, sqlServer.Password);
		}

		public dynamic GetSqlServer()
		{
			var node = this._xmlDoc.SelectSingleNode("./Config/SqlServer");
			return new
			{
				Server = node.Attributes["Server"].Value,
				Database = node.Attributes["Database"].Value,
				Account = node.Attributes["Account"].Value,
				Password = node.Attributes["Password"].Value
			};
		}
		public void SetSqlServer(dynamic data)
		{
			var node = this._xmlDoc.SelectSingleNode("./Config/SqlServer");
			node.Attributes["Server"].Value = data.Server;
			node.Attributes["Database"].Value = data.Database;
			node.Attributes["Account"].Value = data.Account;
			node.Attributes["Password"].Value = data.Password;
		}
		public void Save()
		{
			this._xmlDoc.Save(this._path);
		}
	}
}