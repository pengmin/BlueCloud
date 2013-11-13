using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(new XmlDocument().NameTable);
			nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
			doc.Load(@"E:\Ehrsnet\Web\Source\KPI\Reports\WEBR3221.rdlc");
			doc.SelectSingleNode("./Report", nsmgr);
		}
	}
}
