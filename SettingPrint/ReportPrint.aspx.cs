using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace SettingPrint
{
	public partial class ReportPrint : Page
	{
		private static string ConnStr
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				if (!string.IsNullOrWhiteSpace(Request["id"]) && !string.IsNullOrWhiteSpace(Request["tag"]))
				{
					var dt = GetData(Request["id"].Split(','));
					ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/" + Request["tag"] + ".rdlc");
					ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", dt));
				}
				else
				{
					Response.Redirect("Index.aspx");
				}
			}
		}

		private DataTable GetData(IEnumerable<string> ids)
		{
			var sql =
				@"SELECT d.name AS cln1, '寄件地址' AS cln4,d.officePhoneNo AS cln6,d.mobilePhoneNo AS cln5,
c.contact AS cln21,c.shipmentAddress AS cln24,b.priuserdefnvc2 AS cln25, b.priuserdefnvc1 AS cln27
FROM dbo.SA_SaleDelivery AS a
JOIN dbo.AA_Partner AS b ON b.id=a.idcustomer
JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
JOIN dbo.AA_Person AS d ON d.id=b.idsaleman
WHERE a.id IN({0})";
			var psStr = new StringBuilder();
			var ps = new List<DbParameter>();
			var i = 0;
			foreach (var id in ids)
			{
				if (i != 0)
				{
					psStr.Append(",");
				}
				psStr.Append("@id" + i);
				ps.Add(new SqlParameter("@id" + i, id));
				i++;
			}
			var helper = new SqlHelper(ConnStr);
			helper.Open();
			var dt = helper.GetDataTable(string.Format(sql, psStr), ps.ToArray());
			helper.Close();
			return dt;
		}

		[WebMethod]
		public static string Printed(string id)
		{
			var ids = string.Join("','", id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
			var sql = "update SA_SaleDelivery set priuserdefdecm1=isnull(priuserdefdecm1,0)+1 where id in('" + ids + "')";
			var helper = new SqlHelper(ConnStr);
			helper.Open();
			var v = helper.Execute(sql);
			helper.Close();
			return v.ToString();
		}
	}
}