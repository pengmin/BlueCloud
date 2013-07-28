using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class Certificate
	{
		public Guid RowGuid { get { return Guid.NewGuid(); } }
		public int Iperiod { get { return 1; } }
		public DateTime? Dbill_date { get; set; }
		public int Idock { get { return -1; } }
		public int Ibook { get { return 0; } }
		public int Md_f { get { return 0; } }
		public int Mc_f { get { return 0; } }
		public int Nfrat { get { return 0; } }
		public int Nd_s { get { return 0; } }
		public int Nc_s { get { return 0; } }
		public int BFlagOut { get { return 0; } }
		public int Iyear { get { return DateTime.Today.Year; } }
		public int Iyperiod { get { return DateTime.Today.Year * 100 + 1; } }
		public int Isignseq { get { return 1; } }
		public string Csign { get { return "记"; } }
		public DateTime? Audited { get; set; }
		public string Cbill { get; set; }
		public CertificateItem BorrowItem
		{
			get;
			private set;
		}
		public CertificateItem LendItem
		{
			get;
			private set;
		}

		public void SetItem(CertificateItem borrow, CertificateItem lend)
		{
			if (borrow.Money != lend.Money)
			{
				throw new Exception("借方金额不等于贷方金额");
			}
			borrow.Inid = 1;
			this.BorrowItem = borrow;
			lend.Inid = 2;
			this.LendItem = lend;
		}
	}
}
