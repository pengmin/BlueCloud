using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace SettingPrint
{
	public partial class ReportPrint : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				var dt = BuildDataTable();
				for (var i = 0; i < 5; i++)
				{
					var row = dt.NewRow();
					for (var j = 0; j < dt.Columns.Count; j++)
						row[j] = "test" + i + "_" + j;
					dt.Rows.Add(row);
				}
				this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/tt.rdlc");
				ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", dt));
			}
		}

		private static DataTable BuildDataTable()
		{
			var dt = new DataTable();
			for (var i = 0; i <= 50; i++)
			{
				dt.Columns.Add(new DataColumn("cln" + i));
			}
			return dt;
		}
	}
}