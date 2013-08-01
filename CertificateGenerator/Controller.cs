using Certificate.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateGenerator
{
	public class Controller
	{
		private string _user = null;
		private Config _cfg = new Config(System.Environment.CurrentDirectory + "\\config.sys");

		public Controller(string user)
		{
			this._user = user;
		}

		public void BindReceipts(TreeView ctrl)
		{
			var dir = this._cfg.GetReceiptDirectory();
			ctrl.Nodes[0].Nodes.Clear();
			for (var i = 0; i < dir.GetLength(1); i++)
			{
				TreeNode node = null;
				node = new TreeNode(dir[0, i]);
				node.Tag = dir[1, i];
				ctrl.Nodes[0].Nodes.Add(node);
			}

			ctrl.ExpandAll();
		}

		public void BindReceiptInfo(DataGridView ctrl, string receiptFlag)
		{
			var ado = this._cfg.GetReceiptAdo();
			var sql = this._cfg.GetReceiptInfoSql(receiptFlag);
			ctrl.DataSource = ado.DataTableExecute(sql);
		}
		public void BindReceiptInfo(DataGridView ctrl, string receiptFlag, string site, DateTime start, DateTime end)
		{
			var ado = this._cfg.GetReceiptAdo();
			var sql = this._cfg.GetReceiptInfoSql(receiptFlag);
			ctrl.DataSource = ado.DataTableExecute(sql);
		}

		public void BuildReceiptToCertificate(string receiptFlag, string[] ids)
		{
			var outAdo = this._cfg.GetReceiptAdo();
			var outRep = this._cfg.GetReceiptRepository(this._cfg.GetReceiptAdo(), receiptFlag);
			var inRep = new CertificateInRep(this._cfg.GetCertificateAdo());
			try
			{
				outAdo.Open();
				foreach (var id in ids)
				{
					var cer = outRep.GetCertificate(id);
					cer.Cbill = this._user;
					var cerId = inRep.In(cer);
					if (cerId > -1)
					{
						outAdo.ExecuteNonQuery(string.Format("insert into History values('{0}','{1}','{2}','{3}')", id, cerId, this._user, cer.Dbill_date.Value));
					}
				}
			}
			finally
			{
				outAdo.Close();
			}
		}

		public void GetOutDbInfo(out string server, out string database, out string name, out string pswd)
		{
			this._cfg.GetReceiptDb(out server, out database, out name, out pswd);
		}
		public void SetOutDbInfo(string server, string database, string name, string pswd)
		{
			this._cfg.SetReceiptDb(server, database, name, pswd);
		}

		public void GetInDbInfo(out string server, out string database, out string name, out string pswd)
		{
			this._cfg.GetCertificateDb(out server, out database, out name, out pswd);
			this._cfg.Save();
		}
		public void SetInDbInfo(string server, string database, string name, string pswd)
		{
			this._cfg.SetCertificateDb(server, database, name, pswd);
			this._cfg.Save();
		}

		public void GetReceiptInfo(string flag, out string name, out string infoSql, out string outSql)
		{
			this._cfg.GetReceiptInfo(flag, out name, out infoSql, out outSql);
		}
		public void SaveReceiptInfo(string flag, string name, string infoSql, string outSql)
		{
			this._cfg.SaveReceiptInfo(flag, name, infoSql, outSql);
			this._cfg.Save();
		}
		public void AddReceiptInfo(string flag, string name, string infoSql, string outSql)
		{
			this._cfg.AddReceiptInfo(flag, name, infoSql, outSql);
			this._cfg.Save();
		}
		public void RemoveReceiptInfo(string flag)
		{
			this._cfg.RemoveReceiptInfo(flag);
			this._cfg.Save();
		}

		public void InstallHistoryTable()
		{
			var ado = this._cfg.GetReceiptAdo();
			try
			{
				ado.Open();
				ado.ExecuteNonQuery(this._cfg.GetHistoryTableSql());
			}
			finally
			{
				ado.Close();
			}
		}
	}
}
