using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class AddMemberView : Page
    {
        private readonly AddMemberViewModel _viewModel;

        public AddMemberView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new AddMemberViewModel();
            }

            DataContext = _viewModel;
            AppMessages.MemberValidationMessage.Register(this, OnMessageSent);
            AppMessages.MemberAddedSuccessfullyMessage.Register(this, OnMemberAdded);
            //validationSummary1.Errors.Add(new ValidationSummaryItem("something"));
        }

        private void OnMessageSent(string obj)
        {
            validationSummary1.Errors.Clear();
            validationSummary1.Errors.Add(new ValidationSummaryItem(obj));
        }

        private void OnMemberAdded(string obj)
        {
            validationSummary1.Errors.Clear();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var target = new Uri("/MemberManagementView", UriKind.Relative);
            if (NavigationService != null) NavigationService.Navigate(target);
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }
    }
}