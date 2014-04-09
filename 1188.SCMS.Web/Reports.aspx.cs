using System;

namespace _1188.SCMS.Web
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var report = Request.QueryString["Report"];

            switch (report)
            {
                case "Members":
                    ReportViewer1.LocalReport.ReportPath = "Reports/MembersReport.rdlc";
                    break;
                case "Visitor":
                    ReportViewer1.LocalReport.ReportPath = "Reports/VisitorCardReport.rdlc";
                    break;
                case "Tickets":
                    ReportViewer1.LocalReport.ReportPath = "Reports/TicketsReport.rdlc";
                    break;
                case "Member":
                    ReportViewer1.LocalReport.ReportPath = "Reports/SingleMember.rdlc";
                    break;
                default:
                    break;
            }

            ReportViewer1.DataBind();
        }
    }
}