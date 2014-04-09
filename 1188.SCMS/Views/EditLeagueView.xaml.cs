using System;
using System.Windows;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class EditLeagueView
    {
        readonly EditLeagueViewModel _viewModel;

        public EditLeagueView(int leagueId)
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditLeagueViewModel();
            }

            DataContext = _viewModel;
            _viewModel.LoadDataByLeagueId(leagueId);
            this._viewModel.ValidationErrorsEvent += new EventHandler(OnCustomValidationError);
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void OnCustomValidationError(object sender, EventArgs a)
        {
            var arg = (CustomValidationErrorEventArgs)a;
            if (arg.HasError)
            {
                _viewModel.ShowDialog( arg.ErrorMessage );
            }
            else
                DialogResult = true;
        }



        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}

