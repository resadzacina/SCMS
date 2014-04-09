using System;
using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;

namespace _1188.SCMS.Views
{
    public partial class EditTicketReservation : ChildWindow
    {
        readonly EditTicketReservationViewModel _viewModel;

        public EditTicketReservation(int id, Event selectedEvent)
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditTicketReservationViewModel(selectedEvent);
            }

            DataContext = _viewModel;

            _viewModel.LoadDataByTicketId(Convert.ToInt32(id));
            this._viewModel.TicketValidationErrorsEvent += new EventHandler(OnCustomValidationError);
            AppMessages.TicketAddedMessage.Register(this, OnTicketAdded);
        }

        private void OnTicketAdded(string obj)
        {
            this.DialogResult = true;
        }

        private void OnCustomValidationError(object sender, EventArgs a)
        {
            var arg = (CustomValidationErrorEventArgs)a;
            if (arg.HasError)
            {
                //MessageBox.Show("Rectify validation Errors", "Check validation errors", MessageBoxButton.OK);
                var mess = new MessageWindow(arg.ErrorMessage);
                mess.Show();
                //DialogResult = false;
            }
            else
                DialogResult = true;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

