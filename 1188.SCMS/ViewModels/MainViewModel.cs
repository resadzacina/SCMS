using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;

namespace _1188.SCMS.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _areButtonsVisible;
        public bool AreButtonsVisible 
        {
            get
            {
                return _areButtonsVisible;
            }
            set
            {
                if (_areButtonsVisible == value) return;
                _areButtonsVisible = value;
                OnPropertyChanged("AreButtonsVisible");
            }
        }

        //public bool AreButtonsVisible { get; set; }

        public MainViewModel()
        {
            if (WebContext.Current.User.IsAuthenticated)
                AreButtonsVisible = true;
            else
                AreButtonsVisible = false;
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            AreButtonsVisible = true;
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            AreButtonsVisible = false;
        }
    }
}
