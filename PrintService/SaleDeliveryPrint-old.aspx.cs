using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PrintService
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ReportDocument myReport = new ReportDocument();
			string reportPath = Server.MapPath("~/crystalreport1.rpt");
			myReport.Load(reportPath);

			var table = this.GetData(this.GetTableDataSql());
			table.TableName = "DataTable1";
			DataSet dt1 = new DataSet();
			dt1.Tables.Add(table);

			//绑定数据集，注意，一个报表用一个数据集。
			myReport.SetDataSource(dt1);
			var mainData = this.GetData(this.GetMainDataSql());
			//foreach (ParameterField param in myReport.ParameterFields)
			//{
			//	myReport.SetParameterValue(param.Name, mainData.Rows[0][param.Name].ToString());
			//}
			for (var i = 1; i <= 7; i++)
			{
				myReport.SetParameterValue("info" + i.ToString(), mainData.Rows[0]["info" + i.ToString()].ToString());
			}
			//myReport.PrintToPrinter(1, true, 0, 0);
			CrystalReportViewer1.ReportSource = myReport;
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
		private string GetMainDataSql()
		{
			var sql =
@"SELECT code AS info1,name AS info2,address AS info3,name1 AS info4,SUM(quantity) AS info5,SUM(price) AS info6, maker AS info7
FROM(
	SELECT a.code,c.name,a.address,d.name AS name1,CONVERT(INT,b.quantity) AS quantity,CONVERT(DECIMAL(18,2),b.quantity*b.taxPrice) AS price,a.maker, CONVERT(VARCHAR(10),a.createdtime) AS createdtime
	FROM dbo.SA_SaleDelivery AS a
	LEFT JOIN dbo.SA_SaleDelivery_b AS b ON a.id=b.idSaleDeliveryDTO
	LEFT JOIN dbo.AA_Partner AS c ON a.idsettleCustomer=c.id
	LEFT JOIN dbo.AA_Warehouse AS d ON a.idwarehouse=d.id
	WHERE a.code LIKE '%{0}%'
) AS temp
GROUP BY temp.code,temp.name,temp.address,name1,temp.maker,temp.createdtime";

			return string.Format(sql, this.Request["code"]);
		}
		private DataTable GetData(string sql)
		{
			var conn = new SqlConnection(ConfigHelper.GetInstance(this.Server.MapPath("~/Config.xml")).SqlConnectionString());
			var cmd = conn.CreateCommand();
			var adp = new SqlDataAdapter(cmd);
			var dt = new DataTable();

			cmd.CommandText = sql;
			adp.Fill(dt);

			return dt;
		}
		private string GetTableDataSql()
		{
			var sql =
@"SELECT
	CONVERT(varchar(5),ROW_NUMBER() OVER(ORDER BY specification,freeItem0,name)) AS cln1,
	specification AS cln2,freeItem0 AS cln3, name AS cln4,
	[28]+[29]+[30]+[31]+[32]+[33]+[34]+[35]+[36]+[37]+[38]+[39]+[40] AS cln5,
	[28] AS cln6,[29] AS cln7,[30] AS cln8,[31] AS cln9,[32] AS cln10,[33] AS cln11,[34] AS cln12,
	[35] AS cln13,[36] AS cln14,[37] AS cln15,[38] AS cln16,[39] AS cln17,[40] AS cln18
FROM(
	SELECT
		specification,freeItem0,name,
		SUM([28]) AS [28],SUM([29]) AS [29],SUM([30]) AS [30],SUM([31]) AS [31],SUM([32]) AS [32],SUM([33]) AS [33],SUM([34]) AS [34],
		SUM([35]) AS [35],SUM([36]) AS [36],SUM([37]) AS [37],SUM([38]) AS [38],SUM([39]) AS [39],SUM([40]) AS [40]
	FROM(
		SELECT
			specification,freeItem0,name,
			(CASE WHEN freeItem1='28#' OR freeItem1='S' THEN quantity ELSE 0 END) AS [28],
			(CASE WHEN freeItem1='29#' OR freeItem1='M' THEN quantity ELSE 0 END) AS [29],
			(CASE WHEN freeItem1='30#' OR freeItem1='L' THEN quantity ELSE 0 END) AS [30],
			(CASE WHEN freeItem1='31#' OR freeItem1='XL' THEN quantity ELSE 0 END) AS [31],
			(CASE WHEN freeItem1='32#' OR freeItem1='XXL' THEN quantity ELSE 0 END) AS [32],
			(CASE WHEN freeItem1='33#' OR freeItem1='XXXL' THEN quantity ELSE 0 END) AS [33],
			(CASE WHEN freeItem1='34#' OR freeItem1='XXXXL' THEN quantity ELSE 0 END) AS [34],
			(CASE WHEN freeItem1='35#' THEN quantity ELSE 0 END) AS [35],
			(CASE WHEN freeItem1='36#' THEN quantity ELSE 0 END) AS [36],
			(CASE WHEN freeItem1='37#' THEN quantity ELSE 0 END) AS [37],
			(CASE WHEN freeItem1='38#' THEN quantity ELSE 0 END) AS [38],
			(CASE WHEN freeItem1='39#' THEN quantity ELSE 0 END) AS [39],
			(CASE WHEN freeItem1='40#' THEN quantity ELSE 0 END) AS [40]
		FROM(
			select c.specification,freeItem0,freeItem1,b.name,CONVERT(INT,SUM(quantity)) AS quantity
			from SA_SaleDelivery_b as a 
			left join aa_unit as b on a.idunit=b.id
			left join aa_inventory as c on a.idinventory=c.id
			LEFT JOIN dbo.SA_SaleDelivery AS d ON d.id=a.idSaleDeliveryDTO
			WHERE d.code='{0}'
			GROUP BY c.specification,freeItem0,freeItem1,b.name
		) AS temp
		GROUP BY temp.specification,temp.freeItem0,temp.freeItem1,temp.name,temp.quantity
	) AS temp
	GROUP BY temp.specification, temp.freeItem0,temp.name
) AS temp";
			return string.Format(sql, this.Request["code"]);
		}
	}
}