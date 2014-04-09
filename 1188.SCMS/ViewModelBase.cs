#region

using System;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using _1188.SCMS.Views;

#endregion

namespace _1188.SCMS
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authService = WebContext.Current.Authentication;
        private static bool? _isInDesignMode;
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                    _isInDesignMode = DesignerProperties.IsInDesignTool;

                return _isInDesignMode.Value;
            }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                if (_isLoggedIn == value) return;
                _isLoggedIn = value;
                OnPropertyChanged("IsLoggedIn");
            }
        }

        protected ViewModelBase()
        {
            _authService.LoggedIn += AuthenticationLoggedIn;
            _authService.LoggedOut += AuthenticationLoggedOut;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            // Send an event notification that the property changed
            // This allows the UI to know when one of the items changes
            if (!String.IsNullOrEmpty(propertyName) &&
                    PropertyChanged != null)
                PropertyChanged(this,
                     new PropertyChangedEventArgs(propertyName));
        }

        public void ShowDialog(string message)
        {
            var mess = new MessageWindow(message);
            mess.Show();
            //MessageBox.Show(message);
            //var th = new Thickness(5);

            //var window = new RadWindow
            //{
            //    Content = message,
            //    Header = "Info",
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //    FontSize = 14,
            //    Padding = th
            //};

            //window.ShowDialog();
        }

        public virtual void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = true;
        }

        public virtual void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = false;
        }
    }
}
