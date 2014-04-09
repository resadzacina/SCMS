using System;
using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Controls
{
    public partial class NewMessage : UserControl
    {
        readonly NewMessageViewModel _viewModel;

        public NewMessage()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new NewMessageViewModel();

                _viewModel.From = WebContext.Current.User.Name;
            }

            DataContext = _viewModel;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
