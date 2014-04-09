<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="_1188.SCMS.Web.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            Height="590px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="100%">
            <LocalReport ReportPath="Reports/MembersReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource3" Name="DataSet2" />
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet3" />
                     <rsweb:ReportDataSource DataSourceId="SqlDataSource2" Name="DataSet4" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMembers"
            TypeName="_1188.SCMS.Web.Services.TeamService"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetReportVisitorCards"
            TypeName="_1188.SCMS.Web.Services.VisitorCardService"></asp:ObjectDataSource>
        <br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:aspnetdbEntities %>" 
            SelectCommand="GetTickets" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:aspnetdbEntities %>" 
            SelectCommand="GetMemberByID" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="  " Name="UserId" 
                    QueryStringField="userId" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
          <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
            ConnectionString="<%$ ConnectionStrings:aspnetdbEntities %>" 
            SelectCommand="GetMembers" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
        <br />
    </div>
    </form>
</body>
</html>
