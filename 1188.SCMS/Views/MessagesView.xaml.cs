using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;
using Telerik.Windows.Controls;
using SelectionChangedEventArgs = Telerik.Windows.Controls.SelectionChangedEventArgs;

namespace _1188.SCMS.Views
{
    public partial class MessagesView : Page
    {
        private Storyboard mailMessageSelectedStoryboard;
        private Storyboard newMessageSelectedStoryboard;

        readonly MessagesViewModel _viewModel;

        public MessagesView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new MessagesViewModel();
            }

            DataContext = _viewModel;

            this.mailMessageSelectedStoryboard = LayoutRoot.Resources["MailMessageSelected"] as Storyboard;
            this.newMessageSelectedStoryboard = LayoutRoot.Resources["NewMessageSelected"] as Storyboard;

            //AppMessages.MailFinishedDeleting.Register(this, OnMailDeleted);
            //MessagesTreeView.ItemsSource = _viewModel.InboxMessages;
        }

        //private void OnMailDeleted(string obj)
        //{
        //    //MessagesTreeView.ItemContainerGenerator

        //    var firstItem = MessagesTreeView.ItemContainerGenerator.ContainerFromIndex(0) as RadTreeViewItem;

        //    if (firstItem != null)
        //    {
        //        firstItem.IsSelected = true;
        //    }
        //}

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void outlookBar_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            //var selectedItem = outlookBar.SelectedItem as RadOutlookBarItem;
            //var tag = selectedItem.Tag as string;

            //switch (tag)
            //{
            //    case "Mail":
            //        this.mailMessageSelectedStoryboard.Begin();
            //        break;
            //    default:
            //        break;
            //}
        }

        private void foldersTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadTreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            
            var firstItem = (sender as RadTreeViewItem).ItemContainerGenerator.ContainerFromIndex(0) as RadTreeViewItem;
            if (firstItem != null)
            {
                firstItem.IsSelected = true;
            }
        }

        private void NewMessageSelected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            newMessageSelectedStoryboard.Begin();
        }

        private void InboxSelected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                _viewModel.SelectedMessage = (Message)((RadTreeViewItem)(e.Source)).Item;
            }
            catch (InvalidCastException ex)
            {
            }
            

            mailMessageSelectedStoryboard.Begin();
        }
    }
}
