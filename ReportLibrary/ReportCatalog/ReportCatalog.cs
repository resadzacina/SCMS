namespace ReportLibrary.ReportCatalog
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ReportCatalog.
    /// </summary>
    [Browsable(false)]
    public partial class ReportCatalog : Telerik.Reporting.Report
    {
        public ReportCatalog()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}