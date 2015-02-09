using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
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
				@"SELECT a.id AS cln0,'' AS cln1, '' AS cln2,'' AS cln3, '' AS cln4,'' AS cln5,'' AS cln6, 
c.contact AS cln21,c.shipmentAddress AS cln22,c.mobilePhone AS cln23, c.telephoneNo AS cln24, '' AS cln25,'' AS cln26 FROM dbo.ST_RDRecord AS a
JOIN dbo.AA_Partner AS b ON b.id=a.idpartner
JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
WHERE a.rdDirectionFlag=0 AND a.id IN({0})";
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
	}
}