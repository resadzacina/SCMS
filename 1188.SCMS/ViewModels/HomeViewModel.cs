#region

using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using _1188.SCMS.Interfaces;

#endregion

namespace _1188.SCMS.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
      //  private readonly AuthenticationService _authService = WebContext.Current.Authentication;
       
        private string _contentText;
        public string ContentText
        {
            get
            {
                return _contentText;
            }
            set
            {
                if (_contentText != value)
                {
                    _contentText = value;
                    OnPropertyChanged("ContentText");
                }
            }
        }

      


        Visibility _isUpArrowVisible;

        public Visibility IsUpArrowVisible
        {
            get { return _isUpArrowVisible; }
            set
            {
                if (_isUpArrowVisible != value)
                {
                    _isUpArrowVisible = value;
                    OnPropertyChanged("IsUpArrowVisible");
                }
            }
        }

        Visibility _isLockVisible;

        public Visibility IsLockVisible
        {
            get { return _isLockVisible; }
            set
            {
                if (_isLockVisible != value)
                {
                    _isLockVisible = value;
                    OnPropertyChanged("IsLockVisible");
                }
            }
        }

        public HomeViewModel()
        {
            LoadData();
            //_authService.LoggedIn += AuthenticationLoggedIn;
            //_authService.LoggedOut += AuthenticationLoggedOut;

           
        }

        /// <summary>
        /// Loads the data for the application.
        /// </summary>
        private void LoadData()
        {
            if (IsInDesignModeStatic)
            {
                LoadDesignData();
            }
            else
            {
                // Load home data asynchronously or set initial values

                //If user is not authenticated
                UpdateHomeUI();

            }
        }

        /// <summary>
        /// Loads temporary data for use in the designer.
        /// </summary>
        /// <remarks></remarks>
        private void LoadDesignData()
        {
            //Design mode data
            ContentText = ApplicationStrings.HomePageNotLoggedIn;
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            UpdateHomeUI();
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            UpdateHomeUI();
        }
        
        private void UpdateHomeUI()
        {
            if (!WebContext.Current.User.IsAuthenticated)
            {
                ContentText = ApplicationStrings.HomePageNotLoggedIn;
                IsUpArrowVisible = Visibility.Collapsed;
                IsLockVisible = Visibility.Visible;
            }
            else
            {
                ContentText = ApplicationStrings.HomePageLoggedIn;
                IsUpArrowVisible = Visibility.Visible;
                IsLockVisible = Visibility.Collapsed;
            }
        }
    }
}
