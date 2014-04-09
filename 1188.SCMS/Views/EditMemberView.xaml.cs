using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class EditMemberView : Page
    {
        readonly EditMemberViewModel _viewModel;

        public EditMemberView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditMemberViewModel();
            }

            DataContext = _viewModel;
            this.birthdateDatePicker.DisplayDateEnd = DateTime.Now.AddDays( - 1825 );
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var memberId = NavigationContext.QueryString["MemberId"];
            int memberIdInt;
            if (int.TryParse(memberId, out memberIdInt))
            {
                _viewModel.LoadData(memberIdInt);
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var target = new Uri("/MemberManagementView", UriKind.Relative);
            if (NavigationService != null) NavigationService.Navigate(target);
        }

        private void PhotoBorderDropHandler(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent( DataFormats.FileDrop ))
            {
                FileInfo[] fi = (FileInfo[])e.Data.GetData( DataFormats.FileDrop );

                BitmapImage image = new BitmapImage();
                using (Stream fileStream = fi[0].OpenRead())
                {
                    image.SetSource( fileStream );
                    fileStream.Seek( 0, SeekOrigin.Begin );
                    MemoryStream targetStream = new MemoryStream();
                    fileStream.CopyTo( targetStream );
                    this._viewModel.ImageBytes = targetStream.ToArray();
                }

                ImageBrush brush = new ImageBrush();
                brush.ImageSource = image;
                brush.Stretch = Stretch.UniformToFill;
                memberImage.Source = image;
                this._viewModel.IsDefaultImage = false;
            }
        }

        private void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
