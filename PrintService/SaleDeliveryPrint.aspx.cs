using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace PrintService
{
	public partial class SaleDeliveryPrint : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
			{
				return;
			}
			var reportType = Request["type"];
			BuildReport(reportType);
		}
		private DataTable GetData(string sql)
		{
			var conn = new SqlConnection(ConfigHelper.GetInstance(Server.MapPath("~/Config.xml")).SqlConnectionString());
			var cmd = conn.CreateCommand();
			var adp = new SqlDataAdapter(cmd);
			var dt = new DataTable();

			cmd.CommandText = sql;
			adp.Fill(dt);

			return dt;
		}
		private string GetMainDataSql()
		{
			const string sql = @"SELECT code AS info1,name AS info2,address AS info3,name1 AS info4,SUM(quantity) AS info5,SUM(price) AS info6, maker AS info7,madedate AS info8,memo AS info9
FROM(
	SELECT a.code,c.name,a.address,d.name AS name1,CONVERT(INT,b.quantity) AS quantity,CONVERT(DECIMAL(18,2),b.quantity*b.taxPrice) AS price,a.maker, CONVERT(VARCHAR(10),a.createdtime) AS createdtime,ISNULL(CONVERT(VARCHAR(10),a.madedate),'') AS madedate,a.memo
	FROM dbo.SA_SaleDelivery AS a
	LEFT JOIN dbo.SA_SaleDelivery_b AS b ON a.id=b.idSaleDeliveryDTO
	LEFT JOIN dbo.AA_Partner AS c ON a.idsettleCustomer=c.id
	LEFT JOIN dbo.AA_Warehouse AS d ON a.idwarehouse=d.id
	WHERE a.code LIKE '%{0}%'
) AS temp
GROUP BY temp.code,temp.name,temp.address,name1,temp.maker,temp.createdtime,temp.madedate,temp.memo";

			return string.Format(sql, Request["code"]);
		}

		private string GetTableDataSql()
		{
			const string sql = @"SELECT
	ROW_NUMBER() OVER(ORDER BY specification,freeItem0,name) AS cln1,
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
			(CASE WHEN freeItem1='28#' OR freeItem1='XS' THEN quantity ELSE 0 END) AS [28],
			(CASE WHEN freeItem1='29#' OR freeItem1='S' THEN quantity ELSE 0 END) AS [29],
			(CASE WHEN freeItem1='30#' OR freeItem1='M' THEN quantity ELSE 0 END) AS [30],
			(CASE WHEN freeItem1='31#' OR freeItem1='L' THEN quantity ELSE 0 END) AS [31],
			(CASE WHEN freeItem1='32#' OR freeItem1='XL' THEN quantity ELSE 0 END) AS [32],
			(CASE WHEN freeItem1='33#' OR freeItem1='XXL' THEN quantity ELSE 0 END) AS [33],
			(CASE WHEN freeItem1='34#' OR freeItem1='XXXL' THEN quantity ELSE 0 END) AS [34],
			(CASE WHEN freeItem1='35#' OR freeItem1='4XL' THEN quantity ELSE 0 END) AS [35],
			(CASE WHEN freeItem1='36#' OR freeItem1='5XL' THEN quantity ELSE 0 END) AS [36],
			(CASE WHEN freeItem1='37#' OR freeItem1='6XL' THEN quantity ELSE 0 END) AS [37],
			(CASE WHEN freeItem1='38#' OR freeItem1='7XL' THEN quantity ELSE 0 END) AS [38],
			(CASE WHEN freeItem1='39#' OR freeItem1='8XL' THEN quantity ELSE 0 END) AS [39],
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
			return string.Format(sql, Request["code"]);
		}
		private string GetTableDataSql3()
		{
			const string sql = @"SELECT
	ROW_NUMBER() OVER(ORDER BY specification,freeItem0,name) AS cln1,
	specification AS cln2,freeItem0 AS cln3, name AS cln4,
	[25]+[26]+[27]+[28]+[29]+[30]+[31]+[32]+[33]+[34]+[35]+[36]+[37]+[38]+[39]+[40]+[41]+[42]+[43]+[44]+[45] AS cln5,
	[28] AS cln6,[29] AS cln7,[30] AS cln8,[31] AS cln9,[32] AS cln10,[33] AS cln11,[34] AS cln12,
	[35] AS cln13,[36] AS cln14,[37] AS cln15,[38] AS cln16,[39] AS cln17,[40] AS cln18,
	[25] AS cln19,[26] AS cln20,[27] AS cln21,[41] AS cln22,[42] AS cln23,[43] AS cln24,[44] AS cln25,[45] AS cln26
FROM(
	SELECT
		specification,freeItem0,name,
		SUM([25]) AS [25],SUM([26]) AS [26],SUM([27]) AS [27],
		SUM([28]) AS [28],SUM([29]) AS [29],SUM([30]) AS [30],SUM([31]) AS [31],SUM([32]) AS [32],SUM([33]) AS [33],SUM([34]) AS [34],
		SUM([35]) AS [35],SUM([36]) AS [36],SUM([37]) AS [37],SUM([38]) AS [38],SUM([39]) AS [39],SUM([40]) AS [40],
		SUM([41]) AS [41],SUM([42]) AS [42],SUM([43]) AS [43],SUM([44]) AS [44],SUM([45]) AS [45]
	FROM(
		SELECT
			specification,freeItem0,name,
			(CASE WHEN freeItem1='25#' OR freeItem1='XS' THEN quantity ELSE 0 END) AS [25],
			(CASE WHEN freeItem1='26#' OR freeItem1='S' THEN quantity ELSE 0 END) AS [26],
			(CASE WHEN freeItem1='27#' OR freeItem1='M' THEN quantity ELSE 0 END) AS [27],
			(CASE WHEN freeItem1='28#' OR freeItem1='L' THEN quantity ELSE 0 END) AS [28],
			(CASE WHEN freeItem1='29#' OR freeItem1='XL' THEN quantity ELSE 0 END) AS [29],
			(CASE WHEN freeItem1='30#' OR freeItem1='XXL' THEN quantity ELSE 0 END) AS [30],
			(CASE WHEN freeItem1='31#' OR freeItem1='XXXL' THEN quantity ELSE 0 END) AS [31],
			(CASE WHEN freeItem1='32#' OR freeItem1='4XL' THEN quantity ELSE 0 END) AS [32],
			(CASE WHEN freeItem1='33#' OR freeItem1='5XL' THEN quantity ELSE 0 END) AS [33],
			(CASE WHEN freeItem1='34#' OR freeItem1='6XL' THEN quantity ELSE 0 END) AS [34],
			(CASE WHEN freeItem1='35#' OR freeItem1='7XL' THEN quantity ELSE 0 END) AS [35],
			(CASE WHEN freeItem1='36#' OR freeItem1='8XL' THEN quantity ELSE 0 END) AS [36],
			(CASE WHEN freeItem1='37#' THEN quantity ELSE 0 END) AS [37],
			(CASE WHEN freeItem1='38#' THEN quantity ELSE 0 END) AS [38],
			(CASE WHEN freeItem1='39#' THEN quantity ELSE 0 END) AS [39],
			(CASE WHEN freeItem1='40#' THEN quantity ELSE 0 END) AS [40],
			(CASE WHEN freeItem1='41#' THEN quantity ELSE 0 END) AS [41],
			(CASE WHEN freeItem1='42#' THEN quantity ELSE 0 END) AS [42],
			(CASE WHEN freeItem1='43#' THEN quantity ELSE 0 END) AS [43],
			(CASE WHEN freeItem1='44#' THEN quantity ELSE 0 END) AS [44],
			(CASE WHEN freeItem1='45#' THEN quantity ELSE 0 END) AS [45]
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
			return string.Format(sql, Request["code"]);
		}

		private string GetTableDataSql2()
		{
			const string sql = @"SELECT temp.*,temp.cln19*temp.cln5 AS cln20
FROM(
	SELECT
		ROW_NUMBER() OVER(ORDER BY specification,freeItem0,name) AS cln1,
		specification AS cln2,freeItem0 AS cln3, name AS cln4,
		[28]+[29]+[30]+[31]+[32]+[33]+[34]+[35]+[36]+[37]+[38]+[39]+[40] AS cln5,
		[28] AS cln6,[29] AS cln7,[30] AS cln8,[31] AS cln9,[32] AS cln10,[33] AS cln11,[34] AS cln12,
		[35] AS cln13,[36] AS cln14,[37] AS cln15,[38] AS cln16,[39] AS cln17,[40] AS cln18,price AS cln19
	FROM(
		SELECT
			specification,freeItem0,name,
			SUM([28]) AS [28],SUM([29]) AS [29],SUM([30]) AS [30],SUM([31]) AS [31],SUM([32]) AS [32],SUM([33]) AS [33],SUM([34]) AS [34],
			SUM([35]) AS [35],SUM([36]) AS [36],SUM([37]) AS [37],SUM([38]) AS [38],SUM([39]) AS [39],SUM([40]) AS [40],price
		FROM(
			SELECT
				specification,freeItem0,name,
				(CASE WHEN freeItem1='28#' OR freeItem1='XS' THEN quantity ELSE 0 END) AS [28],
				(CASE WHEN freeItem1='29#' OR freeItem1='S' THEN quantity ELSE 0 END) AS [29],
				(CASE WHEN freeItem1='30#' OR freeItem1='M' THEN quantity ELSE 0 END) AS [30],
				(CASE WHEN freeItem1='31#' OR freeItem1='L' THEN quantity ELSE 0 END) AS [31],
				(CASE WHEN freeItem1='32#' OR freeItem1='XL' THEN quantity ELSE 0 END) AS [32],
				(CASE WHEN freeItem1='33#' OR freeItem1='XXL' THEN quantity ELSE 0 END) AS [33],
				(CASE WHEN freeItem1='34#' OR freeItem1='XXXL' THEN quantity ELSE 0 END) AS [34],
				(CASE WHEN freeItem1='35#' OR freeItem1='4XL' THEN quantity ELSE 0 END) AS [35],
				(CASE WHEN freeItem1='36#' OR freeItem1='5XL' THEN quantity ELSE 0 END) AS [36],
				(CASE WHEN freeItem1='37#' OR freeItem1='6XL' THEN quantity ELSE 0 END) AS [37],
				(CASE WHEN freeItem1='38#' OR freeItem1='7XL' THEN quantity ELSE 0 END) AS [38],
				(CASE WHEN freeItem1='39#' OR freeItem1='8XL' THEN quantity ELSE 0 END) AS [39],
				(CASE WHEN freeItem1='40#' THEN quantity ELSE 0 END) AS [40],
				price
			FROM(
				select c.specification,freeItem0,freeItem1,b.name,CONVERT(INT,SUM(quantity)) AS quantity, CONVERT(DECIMAL(18,2),taxPrice) AS price
				from SA_SaleDelivery_b as a 
				left join aa_unit as b on a.idunit=b.id
				left join aa_inventory as c on a.idinventory=c.id
				LEFT JOIN dbo.SA_SaleDelivery AS d ON d.id=a.idSaleDeliveryDTO
				WHERE d.code='{0}'
				GROUP BY c.specification,freeItem0,freeItem1,b.name, taxPrice
			) AS temp
			GROUP BY temp.specification,temp.freeItem0,temp.freeItem1,temp.name,temp.quantity,temp.price
		) AS temp
		GROUP BY temp.specification, temp.freeItem0,temp.name,temp.price
	) AS temp
) AS temp";
			return string.Format(sql, Request["code"]);
		}
		private string GetTableDataSql4()
		{
			const string sql = @"SELECT temp.*,temp.cln19*temp.cln5 AS cln20
FROM(
	SELECT
		ROW_NUMBER() OVER(ORDER BY specification,freeItem0,name) AS cln1,
		specification AS cln2,freeItem0 AS cln3, name AS cln4,
		[25]+[26]+[27]+[28]+[29]+[30]+[31]+[32]+[33]+[34]+[35]+[36]+[37]+[38]+[39]+[40]+[41]+[42]+[43]+[44]+[45] AS cln5,
		[28] AS cln6,[29] AS cln7,[30] AS cln8,[31] AS cln9,[32] AS cln10,[33] AS cln11,[34] AS cln12,
		[35] AS cln13,[36] AS cln14,[37] AS cln15,[38] AS cln16,[39] AS cln17,[40] AS cln18,price AS cln19,
		[25] as cln21,[26] as cln22,[27] as cln23,[41] as cln24,[42] as cln25,[43] as cln26,[44] as cln27,[45] as cln28
	FROM(
		SELECT
			specification,freeItem0,name,
			SUM([25]) AS [25],
			SUM([26]) AS [26],
			SUM([27]) AS [27],
			SUM([28]) AS [28],SUM([29]) AS [29],SUM([30]) AS [30],SUM([31]) AS [31],SUM([32]) AS [32],SUM([33]) AS [33],SUM([34]) AS [34],
			SUM([35]) AS [35],SUM([36]) AS [36],SUM([37]) AS [37],SUM([38]) AS [38],SUM([39]) AS [39],SUM([40]) AS [40],
			SUM([41]) AS [41],SUM([42]) AS [42],SUM([43]) AS [43],SUM([44]) AS [44],SUM([45]) AS [45],
			price
		FROM(
			SELECT
				specification,freeItem0,name,
				(CASE WHEN freeItem1='25#' OR freeItem1='XS' THEN quantity ELSE 0 END) AS [25],
				(CASE WHEN freeItem1='26#' OR freeItem1='S' THEN quantity ELSE 0 END) AS [26],
				(CASE WHEN freeItem1='27#' OR freeItem1='M' THEN quantity ELSE 0 END) AS [27],
				(CASE WHEN freeItem1='28#' OR freeItem1='L' THEN quantity ELSE 0 END) AS [28],
				(CASE WHEN freeItem1='29#' OR freeItem1='XL' THEN quantity ELSE 0 END) AS [29],
				(CASE WHEN freeItem1='30#' OR freeItem1='XXL' THEN quantity ELSE 0 END) AS [30],
				(CASE WHEN freeItem1='31#' OR freeItem1='XXXL' THEN quantity ELSE 0 END) AS [31],
				(CASE WHEN freeItem1='32#' OR freeItem1='4XL' THEN quantity ELSE 0 END) AS [32],
				(CASE WHEN freeItem1='33#' OR freeItem1='5XL' THEN quantity ELSE 0 END) AS [33],
				(CASE WHEN freeItem1='34#' OR freeItem1='6XL' THEN quantity ELSE 0 END) AS [34],
				(CASE WHEN freeItem1='35#' OR freeItem1='7XL' THEN quantity ELSE 0 END) AS [35],
				(CASE WHEN freeItem1='36#' OR freeItem1='8XL' THEN quantity ELSE 0 END) AS [36],
				(CASE WHEN freeItem1='37#' THEN quantity ELSE 0 END) AS [37],
				(CASE WHEN freeItem1='38#' THEN quantity ELSE 0 END) AS [38],
				(CASE WHEN freeItem1='39#' THEN quantity ELSE 0 END) AS [39],
				(CASE WHEN freeItem1='40#' THEN quantity ELSE 0 END) AS [40],
				(CASE WHEN freeItem1='41#' THEN quantity ELSE 0 END) AS [41],
				(CASE WHEN freeItem1='42#' THEN quantity ELSE 0 END) AS [42],
				(CASE WHEN freeItem1='43#' THEN quantity ELSE 0 END) AS [43],
				(CASE WHEN freeItem1='44#' THEN quantity ELSE 0 END) AS [44],
				(CASE WHEN freeItem1='45#' THEN quantity ELSE 0 END) AS [45],
				price
			FROM(
				select c.specification,freeItem0,freeItem1,b.name,CONVERT(INT,SUM(quantity)) AS quantity, CONVERT(DECIMAL(18,2),taxPrice) AS price
				from SA_SaleDelivery_b as a 
				left join aa_unit as b on a.idunit=b.id
				left join aa_inventory as c on a.idinventory=c.id
				LEFT JOIN dbo.SA_SaleDelivery AS d ON d.id=a.idSaleDeliveryDTO
				WHERE d.code='{0}'
				GROUP BY c.specification,freeItem0,freeItem1,b.name, taxPrice
			) AS temp
			GROUP BY temp.specification,temp.freeItem0,temp.freeItem1,temp.name,temp.quantity,temp.price
		) AS temp
		GROUP BY temp.specification, temp.freeItem0,temp.name,temp.price
	) AS temp
) AS temp";
			return string.Format(sql, Request["code"]);
		}

		private void BuildReport(string flag)
		{
			ReportViewer1.LocalReport.DataSources.Clear();
			switch (flag)
			{
				case "":
				case null:
				case "1":
					ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/SaleDeliveryReport.rdlc");
					ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", GetData(GetTableDataSql())));
					break;
				case "2":
					ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/SaleDeliveryReport2.rdlc");
					ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", GetData(GetTableDataSql2())));
					break;
				case "3":
					ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/SaleDeliveryReport3.rdlc");
					ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", GetData(GetTableDataSql3())));
					break;
				case "4":
					ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/SaleDeliveryReport4.rdlc");
					ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", GetData(GetTableDataSql4())));
					break;
			}
			var mainData = GetData(GetMainDataSql());
			for (var i = 1; i <= 9; i++)
			{
				ReportViewer1.LocalReport.SetParameters(
					new ReportParameter("info" + i, mainData.Rows[0]["info" + i].ToString()));
			}
		}
	}
}