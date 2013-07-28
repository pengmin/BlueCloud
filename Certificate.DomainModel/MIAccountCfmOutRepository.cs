using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class MIAccountCfmOutRepository : ICertificateOutRepository
	{
		private AdoProxy _ado = null;

		public MIAccountCfmOutRepository(AdoProxy ado)
		{
			this._ado = ado;
		}
		public Certificate GetCertificate(string id)
		{
			Certificate result = null;
			var sql = @"select DBill,DChk,StockId,StockName,SupID,AllName,MainAmtTrans from MIAccountCfmMain where BillNo='{0}' and Status=1";
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
						borrow.SubjectId = reader["SupID"].ToString();
						borrow.SubjectName = reader["AllName"].ToString();
						borrow.Summary = "暂估应付账款-" + borrow.SubjectName;
						borrow.Money = (decimal)reader["MainAmtTrans"];
						//设置贷项
						lend.SubjectId = reader["SupID"].ToString();
						lend.SubjectName = reader["AllName"].ToString();
						lend.Summary = "应付账款-" + lend.SubjectName;
						lend.Money = (decimal)reader["MainAmtTrans"];
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
