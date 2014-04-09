namespace ReportLibrary.ReportCatalog
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for Members_Report.
    /// </summary>
    public partial class Members_Report : Telerik.Reporting.Report
    {
        public Members_Report()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            try
            {
                this.ReportParameters[0].Value = "";
                this.ReportParameters[1].Value = "";
            }
            catch { }
            //this.Filters.Add("=Fields.Name", Telerik.Reporting.Data.FilterOperator.Like, "%=Parameters.paraName.Value");
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}