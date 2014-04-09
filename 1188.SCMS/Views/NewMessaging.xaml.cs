using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;

namespace _1188.SCMS.Views
{
    public partial class NewMessaging : Page
    {
        readonly MessagesViewModel _viewModel;
        private Storyboard mailMessageSelectedStoryboard;
        private Storyboard outMailMessageSelectedStoryboard;
        private Storyboard newMessageSelectedStoryboard;

        public NewMessaging()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new MessagesViewModel();
            }

            DataContext = _viewModel;

            this.mailMessageSelectedStoryboard = LayoutRoot.Resources["MailMessageSelected"] as Storyboard;
            this.newMessageSelectedStoryboard = LayoutRoot.Resources["NewMessageSelected"] as Storyboard;
            this.outMailMessageSelectedStoryboard = LayoutRoot.Resources["OutBoxMessageSelected"] as Storyboard;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (WebContext.Current.User.IsAuthenticated)
            //    this._viewModel. = true;
            //else
            //    AreButtonsVisible = false;
        }

        private void InboxSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _viewModel.SelectedMessage = (Message)listBoxInbox.SelectedItem;
            }
            catch (InvalidCastException ex)
            {
            }

            mailMessageSelectedStoryboard.Begin();
        }

        private void OutboxSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _viewModel.SelectedMessage = (Message)listBoxOutbox.SelectedItem;
            }
            catch (InvalidCastException ex)
            {
            }

            outMailMessageSelectedStoryboard.Begin();
        }

        private void ComposeSelected(object sender, RoutedEventArgs e)
        {
            newMessageSelectedStoryboard.Begin();
        }
    }
}
