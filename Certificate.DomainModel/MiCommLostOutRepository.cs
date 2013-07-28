using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MiCommLostOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;
		public MiCommLostOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql =
@"SELECT DBill,StockId,StockName,BillType,MainAmtPur FROM MiCommLostMain WHERE BillNo='{0}'";
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
						borrow.Summary = "借";
						borrow.SubjectId = reader["StockId"].ToString();
						borrow.SubjectName = reader["StockName"].ToString();
						borrow.Money = (decimal)reader["MainAmtPur"];
						//
						lend.Summary = "贷";
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
