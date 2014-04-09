using System.Windows.Controls;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class ReservationsView : Page
    {
        readonly ReservationsViewModel _viewModel;

        public ReservationsView()
        {
            InitializeComponent();

            if (_viewModel == null)
                _viewModel = new ReservationsViewModel();

            DataContext = _viewModel;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void EditTicketClick(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void DeleteTicketClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel.SelectedTicket == null)
                _viewModel.ShowDialog("You must select a ticket in order to delete a ticket");
        }

        private void NewTicketClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel.SelectedEvent == null)
            {
                _viewModel.ShowDialog("You must select an event in order to add ticket reservations");
                return;
            }

            if (_viewModel.SelectedEvent.AvailableTickets == 0)
            {
                _viewModel.ShowDialog("This event has no available tickets");
                return;
            }

            var dialog = new EditTicketReservation(0, _viewModel.SelectedEvent);
            dialog.Show();
        }


    }
}
