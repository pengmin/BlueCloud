<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPrint.aspx.cs" Inherits="SettingPrint.ReportPrint" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>打印</title>
	<script src="Scripts/jquery-1.11.2.min.js"></script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<div>
			<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="100%" Width="100%">
			</rsweb:ReportViewer>
		</div>
	</form>
</body>
</html>
<script>
	//$(function () {
	//	$("#ReportViewer1_ctl05_ctl06_ctl00_ctl00 input").bind("click", function () {
	//		alert(1);
	//	});
	//});
</script>
