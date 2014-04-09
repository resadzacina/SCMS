using System;
using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.ViewModels;
using _1188.SCMS.Web;

namespace _1188.SCMS.Views
{
    public partial class EditVisitorCardView : ChildWindow
    {
        readonly EditVisitorCardViewModel _viewModel;

        public EditVisitorCardView(int id)
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditVisitorCardViewModel();
            }

            DataContext = _viewModel;

            _viewModel.LoadDataByCardId(Convert.ToInt32(id));

            this._viewModel.TeamValidationErrorsEvent += new EventHandler(OnCustomValidationError);
        }

        private void OnCustomValidationError(object sender, EventArgs a)
        {
            var arg = (CustomValidationErrorEventArgs)a;
            if (arg.HasError)
            {
                var mess = new MessageWindow(arg.ErrorMessage);
                mess.Show();
            }
            else
                DialogResult = true;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

