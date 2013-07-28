using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MINPIOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;

		public MINPIOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql = @"select DBill,DChk,StockId,StockName,SupID,AllName,MainAmtTPur from MiNPIMain where BillNo='{0}'";
			try
			{
				this._ado.Open();
				var reader = this._ado.ExecuteReader(string.Format(sql, id));
				try
				{
					var cer = new Certificate();
					var borrow = new CertificateItem();
					var lend = new CertificateItem();
					if (reader.Read())
					{
						//设置凭证信息
						cer.Dbill_date = reader["dbill"] as DateTime?;
						//设置借项
						borrow.SubjectId = reader["stockid"].ToString();
						borrow.SubjectName = reader["stockname"].ToString();
						borrow.Summary = "库存商品-" + borrow.SubjectName;
						borrow.Money = (decimal)reader["MainAmtTPur"];
						//设置贷项
						lend.SubjectId = reader["SupID"].ToString();
						lend.SubjectName = reader["AllName"].ToString();
						lend.Summary = "暂估应付账款-" + lend.SubjectName;
						lend.Money = (decimal)reader["MainAmtTPur"];
						//
						cer.SetItem(borrow, lend);
						//
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
