using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MiCommOverOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;
		public MiCommOverOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql =
@"SELECT DBill,StockId,StockName,BillType,MainAmtPur FROM MiCommOverMain WHERE BillNo='{0}'";
			try
			{
				this._ado.Open();
				var reader = this._ado.ExecuteReader(string.Format(sql, id));
				try
				{
					if (reader.Read())
					{
						var cer = new Certificate();
						var borrow = new CertificateItem();
						var lend = new CertificateItem();
						cer.Dbill_date = reader["DBill"] as DateTime?;
						//
						borrow.Summary = "库存商品";
						borrow.SubjectId = reader["StockId"].ToString();
						borrow.SubjectName = reader["StockName"].ToString();
						borrow.Money = (decimal)reader["MainAmtPur"];
						//
						lend.Summary = "应付账款（自采）";
						lend.SubjectId = reader["StockId"].ToString();
						lend.SubjectName = reader["StockName"].ToString();
						lend.Money = (decimal)reader["MainAmtPur"];
						//
						cer.SetItem(borrow, lend);
						result = cer;
					}
				}
				finally
				{
					reader.Close();
				}
			}
			finally
			{
				this._ado.Close();
			}
			return result;
		}
	}
}
