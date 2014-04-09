#region

using System;
using System.Windows;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.ViewModels;

#endregion

namespace _1188.SCMS.Views
{
    public partial class EditTeamView
    {
        readonly EditTeamViewModel _viewModel;

        public EditTeamView(int teamId)
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditTeamViewModel();
            }

            DataContext = _viewModel;
            _viewModel.LoadDataByTeamId(Convert.ToInt32(teamId));
            _viewModel.LoadSportSocieties();
            this._viewModel.TeamValidationErrorsEvent += new EventHandler( OnCustomValidationError );
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }


        private void OnCustomValidationError( object sender, EventArgs a )
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

