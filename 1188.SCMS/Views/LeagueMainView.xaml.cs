using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _1188.SCMS.Views
{
    public partial class LeagueMainView : Page
    {
        public LeagueMainView()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
        }

        private void LeagueManagementButtonClick( object sender, RoutedEventArgs e )
        {
            Navigate( "LeagueManagementView" );
        }

        private void LeagueAssignmentButtonClick( object sender, RoutedEventArgs e )
        {
            Navigate( "LeagueAssignmentView" );
        }

        private void Navigate( string targetView )
        {
            var target = new Uri( "/" + targetView, UriKind.Relative );
            if ( NavigationService != null ) NavigationService.Navigate( target );
        }
    }
}
