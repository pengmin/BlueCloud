using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class CertificateItem
	{
		public string Summary { get; set; }
		public string SubjectId { get; set; }
		public string SubjectName { get; set; }
		public decimal Money { get; set; }
		public int Inid { get; set; }
		public string Cdept_id { get; set; }
		public string Csup_id { get; set; }
	}
}
