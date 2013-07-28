using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MiStockMoveOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;
		public MiStockMoveOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql =
@"SELECT DBill,StockOut,StockOutName,StockIn,StockInName,MainAmtPur FROM MiStockMoveMain WHERE BillNo='{0}'";
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
						borrow.SubjectId = reader["StockOut"].ToString();
						borrow.SubjectName = reader["StockOutName"].ToString();
						borrow.Money = (decimal)reader["MainAmtPur"];
						borrow.Summary = "库存商品-" + borrow.SubjectName;
						//
						lend.SubjectId = reader["StockIn"].ToString();
						lend.SubjectName = reader["StockInName"].ToString();
						lend.Money = (decimal)reader["MainAmtPur"];
						lend.Summary = "库存商品-" + lend.SubjectName;
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
