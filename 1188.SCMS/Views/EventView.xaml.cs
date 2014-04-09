using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class EventView : ChildWindow
    {
        private readonly EventViewModel _viewModel;

        public EventView(int eventId)
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EventViewModel();
            }

            DataContext = _viewModel;
            _viewModel.LoadDataByEventId(Convert.ToInt32(eventId));
            this._viewModel.EventValidationErrorsEvent += new EventHandler( OnCustomValidationError );
            AppMessages.EventAddedMessage.Register( this, OnEventAdded );
        }


        private void OnEventAdded( string obj )
        {
            this.DialogResult = true;
        }


        private void OnCustomValidationError( object sender, EventArgs a )
        {
            var arg = (CustomValidationErrorEventArgs)a;
            if ( arg.HasError )
            {
                var mess = new MessageWindow( arg.ErrorMessage );
                mess.Show();
            }
            else
                DialogResult = true;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
           // this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

