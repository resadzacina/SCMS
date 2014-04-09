using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _1188.SCMS.Views
{
    public partial class TeamMainView : Page
    {
        public TeamMainView()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
        }

        private void TeamManagementClick( object sender, RoutedEventArgs e )
        {
            NavigateToTeamManagementView();
        }

        private void NavigateToTeamManagementView()
        {
            Navigate( "TeamManagementView" );
        }
     
        private void LeagueAssignmentButtonClick( object sender, RoutedEventArgs e )
        {
            Navigate( "LeagueAssignmentView" );
        }

        private void MemberTeamButtonClick( object sender, RoutedEventArgs e )
        {
            Navigate( "MemberToTeamAssignView" );
        }

        private void Navigate( string targetView )
        {
            var target = new Uri( "/" + targetView, UriKind.Relative );
            if ( NavigationService != null ) NavigationService.Navigate( target );
        }

    }
}
