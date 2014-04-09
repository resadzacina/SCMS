using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;
using _1188.SCMS.Controls;

namespace _1188.SCMS.ViewModels
{
    public class MessagesViewModel : ViewModelBase
    {
        #region Properties

       // public bool IsLoggedIn { get; set; }

        private IEnumerable<Message> _inboxMessages;
        public IEnumerable<Message> InboxMessages
        {
            get
            {
                return _inboxMessages;
            }
            set
            {
                if (_inboxMessages == value) return;
                _inboxMessages = value;
                OnPropertyChanged("InboxMessages");
            }
        }

        private IEnumerable<Message> _outboxMessages;
        public IEnumerable<Message> OutboxMessages
        {
            get
            {
                return _outboxMessages;
            }
            set
            {
                if (_outboxMessages == value) return;
                _outboxMessages = value;
                OnPropertyChanged("OutboxMessages");
            }
        }

        private IEnumerable<string> _users;
        public IEnumerable<string> Users
        {
            get
            {
                return _users;
            }
            set
            {
                if (_users == value) return;
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        private Message _selectedMessage;
        public Message SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }
            set
            {
                if (_selectedMessage == value) return;

                _selectedMessage = value;
                OnPropertyChanged("SelectedMessage");
            }
        }
        #endregion

        private MessageContext _context;
        private UsersContext _usersContext;

        private readonly RelayCommand _deleteMessageCommand;
        public ICommand DeleteMessageCommand { get { return _deleteMessageCommand; } }

        public MessagesViewModel()
        {
            _context = ContextFactory.GetMessageContext();
            _usersContext = ContextFactory.GetUserContext();
            AppMessages.MailDeletedMessage.Register(this, OnMessageDeleted);
            AppMessages.MailSentMessage.Register(this, OnMessageSent);
            IsLoggedIn = true;
            //Load all messages for current user sorted
            LoadData();
        }

        private void OnMessageSent(string obj)
        {
            try
            {
                _context.Load(_context.GetMessagesSentFromUserNameQuery(WebContext.Current.User.Name)).Completed += OnSentMessagesLoadCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void OnMessageDeleted(string s)
        {
            try
            {
                _context.DeleteMessage(SelectedMessage).Completed += OnDeleteCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void OnDeleteCompleted(object sender, EventArgs e)
        {
            ShowDialog("Successfully deleted");
            LoadData();

            //AppMessages.MailFinishedDeleting.Send();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            try
            {
                _context.Load(_context.GetMessagesSentToUserNameQuery(WebContext.Current.User.Name)).Completed += OnInboxMessagesLoadCompleted;
                _context.Load(_context.GetMessagesSentFromUserNameQuery(WebContext.Current.User.Name)).Completed += OnSentMessagesLoadCompleted;
                _usersContext.Load(_usersContext.GetAllUsersQuery()).Completed += UsersLoadCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UsersLoadCompleted(object sender, EventArgs e)
        {
            var ent = ((LoadOperation<aspnet_User>)sender).Entities;

            Users = ent.AsEnumerable().Select(en => en.LoweredUserName);
        }

        private void OnInboxMessagesLoadCompleted(object sender, EventArgs e)
        {
            var messages = ((LoadOperation<Message>)sender).Entities;

            InboxMessages = messages;
        }

        private void OnSentMessagesLoadCompleted(object sender, EventArgs e)
        {
            var messages = ((LoadOperation<Message>)sender).Entities;

            OutboxMessages = messages;
        }

        public override void AuthenticationLoggedIn(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
            IsLoggedIn = true;
        }

        public override void AuthenticationLoggedOut(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
            IsLoggedIn = false;
        }
    }
}
