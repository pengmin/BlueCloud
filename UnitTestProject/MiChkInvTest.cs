using System;
using Certificate.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
	[TestClass]
	public class MiChkInvTest
	{
		[TestMethod]
		public void OutTest()
		{
			var ado = new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True");
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiChkInv, ado);
			var cer = rep.GetCertificate("0001-13070300001");
		}
		[TestMethod]
		public void InTest()
		{
			var ado = new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True");
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiChkInv, ado);
			var cer = rep.GetCertificate("0001-13070300001");

			var inRep = new CertificateInRep(new AdoProxy("Data Source=.;Initial Catalog=U8DB;Integrated Security=True"));
			inRep.In(cer);
		}
	}
}
