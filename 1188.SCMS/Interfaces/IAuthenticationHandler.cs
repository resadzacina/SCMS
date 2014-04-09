using System;
using System.Net;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace _1188.SCMS.Interfaces
{
    public interface ILogHandler
    {
        void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e);
        void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e);
    }
}
