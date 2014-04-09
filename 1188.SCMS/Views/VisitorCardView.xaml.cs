using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class VisitorCardView : Page
    {
        private readonly VisitorCardViewModel _viewModel;

        public VisitorCardView()
        {
            InitializeComponent();

            if (_viewModel == null)
                _viewModel = new VisitorCardViewModel();

            DataContext = _viewModel;
        }

        private void NewVisitorCard_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditVisitorCardView(0);
            dialog.Show();
        }
    }
}
