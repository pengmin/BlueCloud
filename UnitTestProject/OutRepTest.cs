using System;
using Certificate.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
	[TestClass]
	public class OutRepTest
	{
		[TestMethod]
		public void TestMiCommOver()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiCommOver, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("0001-13070400001");
		}
		[TestMethod]
		public void TestMiCommLost()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiCommLost, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("0001-13070400001");
		}
		[TestMethod]
		public void TestMiStockMove()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiStockMove, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("0001-13070400001");
		}
		[TestMethod]
		public void TestMINPI()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MINPI, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("01-13070600001");
		}
		[TestMethod]
		public void TestMIPI()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MiPI, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("01-13070600001");
		}
		[TestMethod]
		public void TestMIAccountCfm()
		{
			var rep = CertificateOutFactory.GetRepository(CertificateCategory.MIAccountCfm, new AdoProxy("Data Source=.;Initial Catalog=CMIPSDB;Integrated Security=True"));
			var cer = rep.GetCertificate("01-13070600001");
		}
	}
}
