using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace _1188.SCMS.Views
{
    public partial class ReportingView : Page
    {
        public ReportingView()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenReportPopup("Members");
        }

        private static void OpenReportPopup(string report)
        {
            var options = new HtmlPopupWindowOptions
                              {
                                  Left = 0,
                                  Top = 0,
                                  Width = 930,
                                  Height = 800,
                                  Menubar = false,
                                  Toolbar = false,
                                  Directories = false,
                                  Status = false
                              };

            //Button btn = sender as Button;

            //int OrderID = int.Parse(btn.Content.ToString());

            string address = Application.Current.Host.Source.AbsoluteUri;
            int i = address.IndexOf("/ClientBin/", 1);
            string url = address.Substring(0, i);
            url = url + string.Format("/Reports.aspx?Report={0}", report);

            if (true == HtmlPage.IsPopupWindowAllowed) HtmlPage.PopupWindow(new Uri(url), "new", options);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            OpenReportPopup("Visitor");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            OpenReportPopup("Tickets");
        }
    }
}
