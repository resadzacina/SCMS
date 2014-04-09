namespace ReportLibrary.ReportCatalog
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Summary description for Visitor_Report.
    /// </summary>
    public partial class Visitor_Report : Telerik.Reporting.Report
    {
        public Visitor_Report()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
        }
    }
}