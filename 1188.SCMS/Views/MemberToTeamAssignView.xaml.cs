using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class MemberToTeamAssignView : Page
    {
        readonly MemberToTeamAssignViewModel _viewModel;

        public MemberToTeamAssignView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new MemberToTeamAssignViewModel();
            }

            DataContext = _viewModel;
        }

        private void NewMemberTeamuttonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditMemberTeamView();
            dialog.Show();
        }
    }
}
