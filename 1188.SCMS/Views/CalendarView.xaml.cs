using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;

namespace _1188.SCMS.Views
{
    public partial class CalendarView
    {
        readonly CalendarViewModel _viewModel;

        public CalendarView()
        {
            InitializeComponent();

            if (_viewModel == null)
                _viewModel = new CalendarViewModel();

            DataContext = _viewModel;
        }

        private void EventsListFilter(object sender, FilterEventArgs e)
        {
            var item = e.Item as Event;
            e.Accepted = calEvent != null && this.calEvent.SelectedDates.Contains(item.DateStart);
        }

        private void TeamListFilter(object sender, FilterEventArgs e)
        {
            var item = e.Item as Team;
            e.Accepted = true;
        }

        private void NewEventButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new EventView(0);
            dialog.Show();
        }

        private void EditEventButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel.SelectedEvent == null)
            {
                _viewModel.ShowDialog( "Event not selected" );
                return;
            }

            var id = _viewModel.SelectedEvent.ID;
            var dialog = new EventView(id);
            dialog.Show();
        }

        private void DeleteEventButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void calEvent_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var collection = (this.Resources["EventsView"] as CollectionViewSource);

            if (collection.View != null)
            {
                collection.View.Refresh();

                _viewModel.CheckSelection( calEvent.SelectedDate );
            }
        }
    }
}
