using Certificate.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CertificateGenerator
{
	public class Config
	{
		private XmlDocument _xmlDoc = new XmlDocument();
		private string _path = null;

		public Config(string path)
		{
			this._path = path;
			this._xmlDoc.Load(this._path);
		}

		public void Save()
		{
			this._xmlDoc.Save(this._path);
		}

		public string[,] GetReceiptDirectory()
		{
			var xNodes = this._xmlDoc.SelectNodes("./config/receipts/receipt");
			if (xNodes.Count > 0)
			{
				var dir = new string[2, xNodes.Count];
				for (var i = 0; i < xNodes.Count; i++)
				{
					dir[0, i] = xNodes[i].Attributes["name"].Value;
					dir[1, i] = xNodes[i].Attributes["flag"].Value;
				}
				return dir;
			}
			else
			{
				return new string[2, 0];
			}
		}

		public void GetReceiptInfo(string flag, out string name, out string infoSql, out string outSql)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/receipts/receipt[@flag='" + flag + "']");
			name = xNode.Attributes["name"].Value;
			infoSql = this.GetInfoSql(flag);
			outSql = this.GetOutSql(flag);
		}
		public void SaveReceiptInfo(string flag, string name, string infoSql, string outSql)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/receipts/receipt[@flag='" + flag + "']");
			xNode.Attributes["name"].Value = name;
			var infoSqlNode = xNode.SelectSingleNode("infoSql");
			infoSqlNode.InnerXml = "<![CDATA[" + infoSql + "]]>";
			var outSqlNode = xNode.SelectSingleNode("outSql");
			outSqlNode.InnerXml = "<![CDATA[" + outSql + "]]>";
		}
		public void AddReceiptInfo(string flag, string name, string infoSql, string outSql)
		{
			var nodeTemp = @"<receipt name='{0}' flag='{1}'><infoSql><![CDATA[{2}]]></infoSql><outSql><![CDATA[{3}]]></outSql></receipt>";
			var receiptsNode = this._xmlDoc.SelectSingleNode("./config/receipts");
			receiptsNode.InnerXml += string.Format(nodeTemp, name, flag, infoSql, outSql);
		}
		public void RemoveReceiptInfo(string flag)
		{
			var receiptsNode = this._xmlDoc.SelectSingleNode("./config/receipts");
			var receiptNode = this._xmlDoc.SelectSingleNode("./config/receipts/receipt[@flag='" + flag + "']");
			receiptsNode.RemoveChild(receiptNode);
		}

		public string GetReceiptInfoSql(string receiptFlag)
		{
			return this.GetInfoSql(receiptFlag);
		}
		public string GetReceiptOutSql(string receiptFlag)
		{
			return this.GetOutSql(receiptFlag);
		}

		public void SetReceiptDb(string server, string database, string name, string pswd)
		{
			this.SetDbInfo("outDbInfo", server, database, name, pswd);
		}
		public void GetReceiptDb(out string server, out string database, out string name, out string pswd)
		{
			this.GetDbInfo("outDbInfo", out server, out database, out name, out pswd);
		}


		public void SetCertificateDb(string server, string database, string name, string pswd)
		{
			this.SetDbInfo("inDbInfo", server, database, name, pswd);
		}
		public void GetCertificateDb(out string server, out string database, out string name, out string pswd)
		{
			this.GetDbInfo("inDbInfo", out server, out database, out name, out pswd);
		}

		public AdoProxy GetReceiptAdo()
		{
			return new AdoProxy(this.GetDbConnStr("outDbInfo"));
		}
		public AdoProxy GetCertificateAdo()
		{
			return new AdoProxy(this.GetDbConnStr("inDbInfo"));
		}

		public ICertificateOutRepository GetReceiptRepository(AdoProxy ado, string receiptFlag)
		{
			return new ConfigOutRep(ado, this.GetOutSql(receiptFlag));
		}

		public string GetHistoryTableSql()
		{
			return this._xmlDoc.SelectSingleNode("./config/HistoryTable").InnerText;
		}

		private void GetDbInfo(string node, out string server, out string database, out string name, out string pswd)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/" + node);
			if (xNode != null)
			{
				server = xNode.Attributes["server"].Value;
				database = xNode.Attributes["database"].Value;
				name = xNode.Attributes["name"].Value;
				pswd = xNode.Attributes["pswd"].Value;
			}
			else
			{
				server = null;
				database = null;
				name = null;
				pswd = null;
			}
		}
		private void SetDbInfo(string node, string server, string database, string name, string pswd)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/" + node);
			if (xNode != null)
			{
				xNode.Attributes["server"].Value = server;
				xNode.Attributes["database"].Value = database;
				xNode.Attributes["name"].Value = name;
				xNode.Attributes["pswd"].Value = pswd;
			}
		}
		private string GetDbConnStr(string node)
		{
			string server, database, name, pswd;
			this.GetDbInfo(node, out server, out database, out name, out pswd);
			return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", server, database, name, pswd);
		}
		private string GetInfoSql(string receiptFlag)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/receipts/receipt[@flag='" + receiptFlag + "']/infoSql");
			if (xNode != null)
			{
				return xNode.InnerText;
			}
			else
			{
				return string.Empty;
			}
		}
		private string GetOutSql(string receiptFlag)
		{
			var xNode = this._xmlDoc.SelectSingleNode("./config/receipts/receipt[@flag='" + receiptFlag + "']/outSql");
			if (xNode != null)
			{
				return xNode.InnerText;
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
