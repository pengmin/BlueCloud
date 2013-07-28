using Certificate.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateGenerator
{
	public class ConfigOutRep : ICertificateOutRepository
	{
		private AdoProxy _ado = null;
		private string _sql = null;

		public ConfigOutRep(AdoProxy ado, string sql)
		{
			this._ado = ado;
			this._sql = sql;
		}

		public Certificate.DomainModel.Certificate GetCertificate(string id)
		{
			Certificate.DomainModel.Certificate result = null;
			try
			{
				this._ado.Open();
				var reader = this._ado.ExecuteReader(string.Format(this._sql, id));
				try
				{
					if (reader.Read())
					{
						var cer = new Certificate.DomainModel.Certificate();
						var borrow = new Certificate.DomainModel.CertificateItem();
						var lend = new Certificate.DomainModel.CertificateItem();
						cer.Dbill_date = reader["created"] as DateTime?;
						cer.Audited = reader["audited"] as DateTime?;
						cer.Cbill = reader["createUser"] as string;
						//
						borrow.Summary = reader["borrowRemark"] as string;
						borrow.SubjectId = reader["borrowSubject"] as string;
						//borrow.SubjectName = reader["borrowSubjectName"] as string;
						borrow.Money = reader["borrowMoney"] is decimal ? (decimal)reader["borrowMoney"] : 0;
						try
						{
							borrow.Cdept_id = reader["borrowDept"] as string;
						}
						catch
						{
						}
						try
						{
							borrow.Csup_id = reader["borrowSup"] as string;
						}
						catch
						{
						}
						//
						lend.Summary = reader["lendRemark"] as string;
						lend.SubjectId = reader["lendSubject"] as string;
						//lend.SubjectName = reader["lendSubjectName"] as string;
						lend.Money = reader["lendMoney"] is decimal ? (decimal)reader["lendMoney"] : 0;
						try
						{
							lend.Cdept_id = reader["lendDept"] as string;
						}
						catch
						{
						}
						try
						{
							lend.Csup_id = reader["lendSup"] as string;
						}
						catch
						{
						}
						//
						cer.SetItem(borrow, lend);
						//
						result = cer;
					}
				}
				catch (Exception e)
				{
					throw e;
				}
				finally
				{
					reader.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				this._ado.Close();
			}
			return result;
		}
	}
}
