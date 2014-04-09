#region

using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;

#endregion

namespace _1188.SCMS.Views
{
    public partial class DataManagementView
    {
        readonly DataManagementViewModel _viewModel;
        public DataManagementView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new DataManagementViewModel();
            }

            DataContext = _viewModel;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void TeamManagementClick(object sender, RoutedEventArgs e)
        {
            NavigateToTeamManagementView();
        }

        private void NavigateToTeamManagementView()
        {
            Navigate("TeamManagementView");
        }

        private void LeagueManagementButtonClick(object sender, RoutedEventArgs e)
        {
            Navigate("LeagueManagementView");
        }

        private void LeagueAssignmentButtonClick(object sender, RoutedEventArgs e)
        {
            Navigate("LeagueAssignmentView");
        }

        private void MemberManagementButtonClick(object sender, RoutedEventArgs e)
        {
            Navigate("MemberManagementView");
        }

        private void MemberTeamButtonClick(object sender, RoutedEventArgs e)
        {
            Navigate("MemberToTeamAssignView");
        }

        private void Navigate(string targetView)
        {
            var target = new Uri("/" + targetView, UriKind.Relative);
            if (NavigationService != null) NavigationService.Navigate(target);
        }

        private void GenerateReportClick( object sender, RoutedEventArgs e )
        {
            OpenReportPopup( "Members" );
        }

        private static void OpenReportPopup( string report )
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
            int i = address.IndexOf( "/ClientBin/", 1 );
            string url = address.Substring( 0, i );
            url = url + string.Format( "/Reports.aspx?Report={0}", report );

            if ( true == HtmlPage.IsPopupWindowAllowed ) HtmlPage.PopupWindow( new Uri( url ), "new", options );
        }

    }
}
