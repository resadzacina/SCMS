using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;

namespace _1188.SCMS.Views
{
    public partial class MemberManagementView
    {
        readonly MemberManagementViewModel _viewModel;

        public MemberManagementView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new MemberManagementViewModel();
            }

            DataContext = _viewModel;
            AppMessages.NavigateToEditMemberMessage.Register( this, OnNavigateMessage );
        }

        private void OnNavigateMessage( string obj )
        {
            NavigateToEditEvent(Convert.ToInt32(obj));
        }

        private void NewMemberClick(object sender, RoutedEventArgs e)
        {
            var target = new Uri("/AddMemberView", UriKind.Relative);
            if (NavigationService != null) NavigationService.Navigate(target);
        }

        private void NavigateToEditEvent( int memberId )
        {
            var target = new Uri( "/EditMemberView?MemberId=" + memberId, UriKind.Relative );
            if ( NavigationService != null ) NavigationService.Navigate( target );
        }

        private void GenerateReportClick(object sender, RoutedEventArgs e)
        {
            var selectedMember = memberDataGrid.SelectedItem as Member;

            if (selectedMember == null)
            {
                var mess = new MessageWindow("Memnber is not selected");
                mess.Show();
                return;
            }

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

            var address = Application.Current.Host.Source.AbsoluteUri;
            var i = address.IndexOf("/ClientBin/", 1);
            var url = address.Substring(0, i);
            url = url + "/Reports.aspx?Report=Member&userId=" + selectedMember.UserID;

            if (true == HtmlPage.IsPopupWindowAllowed) HtmlPage.PopupWindow(new Uri(url), "new", options);
        }
    }
}
