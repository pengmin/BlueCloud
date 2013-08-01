using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class CertificateInRep
	{
		private const string SQL =
@"insert into GL_accvouch(RowGuid,iperiod,inid,dbill_date,idoc,ibook,
	ccode,md ,mc ,
	md_f,mc_f,nfrat,nd_s,nc_s,bFlagOut,iyear,iyperiod,isignseq,
	ino_id,
	csign,cdigest,cdept_id,csup_id,cbill)
values(newid(),{13},{0},'{1}',-1,0,
		'{2}',{3},{4},
		0.00,0.00,0,0,0,0,{5},{6},1,
		{7},
		'{8}','{9}',{10},{11},'{12}')";

		private AdoProxy _ado = null;

		public CertificateInRep(AdoProxy ado)
		{
			this._ado = ado;
		}
		public int In(Certificate cer)
		{
			var ino_id = this.Ino_id();
			StringBuilder sql = new StringBuilder();
			//创建借凭证
			sql.Append(string.Format(SQL, 1, cer.Dbill_date,
				cer.BorrowItem.SubjectId, cer.BorrowItem.Money, 0,
				cer.Iyear, cer.Iyperiod,
				ino_id,
				cer.Csign, cer.BorrowItem.Summary,
				string.IsNullOrEmpty(cer.BorrowItem.Cdept_id) ? "NULL" : "'" + cer.BorrowItem.Cdept_id + "'",
				string.IsNullOrEmpty(cer.BorrowItem.Csup_id) ? "NULL" : "'" + cer.BorrowItem.Csup_id + "'",
				cer.Cbill,
				cer.Dbill_date != null && cer.Dbill_date.HasValue ? cer.Dbill_date.Value.Month : 1));
			//创建贷凭证
			sql.Append(string.Format(SQL, 2, cer.Dbill_date,
				cer.LendItem.SubjectId, 0, cer.LendItem.Money,
				cer.Iyear, cer.Iyperiod,
				ino_id,
				cer.Csign, cer.LendItem.Summary,
				string.IsNullOrEmpty(cer.LendItem.Cdept_id) ? "NULL" : "'" + cer.LendItem.Cdept_id + "'",
				string.IsNullOrEmpty(cer.LendItem.Csup_id) ? "NULL" : "'" + cer.LendItem.Csup_id + "'",
				cer.Cbill,
				cer.Dbill_date != null && cer.Dbill_date.HasValue ? cer.Dbill_date.Value.Month : 1));
			//入库
			try
			{
				this._ado.Open();
				this._ado.ExecuteNonQuery(sql.ToString());

				return ino_id;
			}
			catch
			{
				return -1;
			}
			finally
			{
				this._ado.Close();
			}
		}
		private int Ino_id()
		{
			try
			{
				this._ado.Open();
				return (int)this._ado.ExecuteScalar("(select isnull(max(ino_id),0)+1 from GL_accvouch)");
			}
			finally
			{
				this._ado.Close();
			}
		}
	}
}
