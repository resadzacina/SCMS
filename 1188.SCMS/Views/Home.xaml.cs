using System;

namespace _1188.SCMS
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using _1188.SCMS.ViewModels;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        HomeViewModel _ViewModel;
        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            if (_ViewModel == null)
            {
                _ViewModel = new HomeViewModel();
            }
            
            this.DataContext = _ViewModel;
            this.Title = ApplicationStrings.HomePageTitle;
            AppMessages.NavigateToEditMemberMessage.Register( this, OnNavigateMessage );
        }

        private void OnNavigateMessage( string obj )
        {
            var target = new Uri( "/EditMemberView?MemberId=" + obj, UriKind.Relative );
            if ( NavigationService != null ) NavigationService.Navigate( target );
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}