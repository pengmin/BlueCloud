<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SettingPrint.Index" %>

<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>销货单</title>
	<script src="Scripts/jquery-1.11.2.min.js"></script>
	<style>
		.gridView {
			border-collapse: collapse;
			border: solid 1px #aaa;
		}

			.gridView .head {
				background-color: #ddd;
			}

			.gridView td {
				border: solid 1px #aaa;
			}
	</style>
</head>
<body>
	<form method="POST">
		<div>
			<a href="javascript:print('tt');">打印天天快递单</a>
			<a href="javascript:print('st');">打印畅通快递单</a>
			<a href="javascript:print('sf');">打印顺丰快递单</a>
			<a href="javascript:print('db');">打印德邦快递单</a>
		</div>
		<table class="gridView">
			<thead class="head">
				<tr>
					<th>
						<input type="checkbox" onclick="toggleAll(this);" /></th>
					<%
						foreach (DataColumn cln in Data.Columns)
						{
							if (cln.ColumnName == "id") continue;
					%>
					<th><%=cln.ColumnName %></th>
					<%
						}
					%>
				</tr>
			</thead>
			<tbody>
				<%
					foreach (DataRow row in Data.Rows)
					{
				%>
				<tr>
					<td>
						<input type="checkbox" value="<%=row["id"] %>" name="id" /></td>
					<%
						foreach (DataColumn cln in Data.Columns)
						{
							if (cln.ColumnName == "id") continue;
					%>
					<td><%=row[cln] %></td>
					<%
						}
					%>
				</tr>
				<%
					}
				%>
			</tbody>
		</table>
		<div>
			<%
				for (var i = 1; i <= PageCount; i++)
				{
			%>
			<span><a href="/Index.aspx?page=<%=i %>"><%=i %></a></span>
			<%
				}
			%>
		</div>
	</form>
</body>
</html>
<script>
	function toggleAll(el) {
		var checked = $(el).prop("checked");
		$(".gridView tbody :checkbox").each(function () {
			$(this).prop("checked", checked);
		});
	}
	function print(tag) {
		$("form").attr("action", "/ReportPrint.aspx?tag=" + tag);
		$("form").submit();
	}
</script>
