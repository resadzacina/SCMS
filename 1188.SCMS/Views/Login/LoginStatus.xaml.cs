using System;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;
using System.Windows.Navigation;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.LoginUI
{
    using System.Globalization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="UserControl"/> class that shows the current login status and allows login and logout.
    /// </summary>
    public partial class LoginStatus : UserControl
    {
        private readonly AuthenticationService authService = WebContext.Current.Authentication;
        private readonly TeamContext _memberContext;
        /// <summary>
        /// Creates a new <see cref="LoginStatus"/> instance.
        /// </summary>
        public LoginStatus()
        {
            this.InitializeComponent();

            this.welcomeText.SetBinding(TextBlock.TextProperty, WebContext.Current.CreateOneWayBinding("User.DisplayName", new StringFormatValueConverter(ApplicationStrings.WelcomeMessage)));
            this.authService.LoggedIn += this.Authentication_LoggedIn;
            this.authService.LoggedOut += this.Authentication_LoggedOut;
            this.UpdateLoginState();
            this.welcomeText.MouseLeftButtonDown +=new System.Windows.Input.MouseButtonEventHandler(OnUserNameClicked);
            _memberContext = new TeamContext();
        }

        public void OnUserNameClicked(object sender, MouseButtonEventArgs e)
        {
            _memberContext.Load( _memberContext.GetMemberByNameQuery( WebContext.Current.User.DisplayName ), delegate( LoadOperation<Member> operation )
            {
                if ( operation.HasError )
                {
                    operation.MarkErrorAsHandled();
                }
                else
                {
                    var loadedMember = operation.Entities.FirstOrDefault();
                    AppMessages.NavigateToEditMemberMessage.Send( loadedMember.ID.ToString() );
                }


            }, null );

           
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRegistrationWindow loginWindow = new LoginRegistrationWindow();
            loginWindow.Show();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.authService.Logout(logoutOperation =>
            {
                if (logoutOperation.HasError)
                {
                    ErrorWindow.CreateNew(logoutOperation.Error);
                    logoutOperation.MarkErrorAsHandled();
                }
            }, /* userState */ null);
        }

        private void Authentication_LoggedIn(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();
        }

        private void Authentication_LoggedOut(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();
        }

        private void UpdateLoginState()
        {
            if (WebContext.Current.User.IsAuthenticated)
            {
                VisualStateManager.GoToState(this, (WebContext.Current.Authentication is WindowsAuthentication) ? "windowsAuth" : "loggedIn", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "loggedOut", true);
            }
        }
    }
}
