using System;
using System.Windows;
using System.Windows.Controls;

namespace _1188.SCMS.Controls
{
    public partial class MailMessage : UserControl
    {
        public event EventHandler<EventArgs> MessageDeleted;

        public MailMessage()
        {
            InitializeComponent();
        }

        private void DeleteMessageClick(object sender, RoutedEventArgs e)
        {
            AppMessages.MailDeletedMessage.Send();
        }
    }
}
