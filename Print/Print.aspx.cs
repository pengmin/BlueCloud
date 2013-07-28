using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//http://www.cr173.com/html/18645_1.html
namespace Print
{
	public partial class Print : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.ReportViewer1.LocalReport.ReportPath = this.Server.MapPath("~/Print.rdlc");
				this.ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet", this.GetDataset()));
			}
		}
		private DataTable GetDataset()
		{
			var tableData = new DataTable();
			tableData.TableName = "TableData";
			for (var i = 1; i <= 18; i++)
			{
				tableData.Columns.Add("cln" + i.ToString(), typeof(string));
			}
			for (var i = 0; i < 100; i++)
			{
				var row = tableData.NewRow();
				for (var j = 1; j <= 18; j++)
				{
					row["cln" + j.ToString()] = i * 100 + j;
				}
				tableData.Rows.Add(row);
			}
			return tableData;
		}
		private DataTable GetMainData()
		{
			var tableData = new DataTable();
			tableData.TableName = "MainInfo";
			for (var i = 1; i <= 7; i++)
			{
				tableData.Columns.Add("cln" + i.ToString(), typeof(string));
			}
			for (var i = 0; i < 1; i++)
			{
				var row = tableData.NewRow();
				for (var j = 1; j <= 7; j++)
				{
					row["cln" + j.ToString()] = i * 100 + j;
				}
				tableData.Rows.Add(row);
			}
			return tableData;
		}
	}
}