using System.Windows;
using System.Windows.Controls;

namespace _1188.SCMS.Views
{
    public partial class MessageWindow : ChildWindow
    {
        public MessageWindow(string message)
        {
            InitializeComponent();
            messageLabel.Text = message;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

