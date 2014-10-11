<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleDeliveryPrint.aspx.cs" Inherits="PrintService.SaleDeliveryPrint" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<style>
		#top {
			position: fixed;
			_position: absolute;
			top: 5px;
			right: 5px;
			_bottom: auto;
			_top: expression(eval(document.documentElement.scrollTop+5));
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager runat="server"></asp:ScriptManager>
		<div id="top">

			<table id="TABLE1">
				<tbody>
					<tr>
						<td>
							<input id="type_0" type="radio" value="1" name="type" onclick="change(1);" <%=Request["type"]=="1"?"checked='checked'":"" %> /><label for="type_0">无单价金额</label></td>
						<td>
							<input id="type_1" type="radio" value="2" name="type" onclick="change(2);" <%=Request["type"]=="2"?"checked='checked'":"" %> /><label for="type_1">带单价金额</label></td>
						<td>
							<input id="type_2" type="radio" value="3" name="type" onclick="change(3);" <%=Request["type"]=="3"?"checked='checked'":"" %> /><label for="type_2">无单价金额（新）</label></td>
						<td>
							<input id="type_3" type="radio" value="4" name="type" onclick="change(4);" <%=Request["type"]=="4"?"checked='checked'":"" %> /><label for="type_3">带单价金额（新）</label></td>
					</tr>
				</tbody>
			</table>
		</div>
		<div>
			<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="100%" Width="100%" PageCountMode="Actual" ShowExportControls="true" ShowFindControls="False">
			</rsweb:ReportViewer>
		</div>
	</form>
</body>
</html>
<script>
	function change(type) {
		window.location.href = "saledeliveryprint.aspx?type=" + type;
	}
</script>
