using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MiChkInvOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;

		public MiChkInvOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql =
@"select dbill,dchk,stockid,stockname,
	(select sum(itemamttpur) from michkinvitem where billnoid=michkinvmain.billnoid) as m,
	(select sum(qfact)-sum(amtaccount) from michkinvitem where billnoid=michkinvmain.billnoid ) as f
	from michkinvmain where billno='{0}'";

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
						borrow.Summary = ((decimal)reader["f"]) >= 0 ? "库存商品" : "其他应付款-" + borrow.SubjectName;
						borrow.Money = (decimal)reader["m"];
						//设置贷项
						lend.SubjectId = reader["stockid"].ToString();
						lend.SubjectName = reader["stockname"].ToString();
						lend.Summary = ((decimal)reader["f"]) >= 0 ? "其他应付款-" + lend.SubjectName : "库存商品-" + lend.SubjectName;
						lend.Money = (decimal)reader["m"];
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
