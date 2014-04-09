using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class NewMessageViewModel : ViewModelBase
    {
       

        #region Properties
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

        //private Message _newMessage;
        //public Message NewMessage
        //{
        //    get
        //    {
        //        return _newMessage;
        //    }
        //    set
        //    {
        //        if (_newMessage == value) return;
        //        _newMessage = value;
        //        OnPropertyChanged("NewMessage");
        //    }
        //}

        private string _to;
        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                if (_to == value) return;
                _to = value;
                OnPropertyChanged("To");
            }
        }

        private string _from;
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                if (_from == value) return;
                _from = value;
                OnPropertyChanged("From");
            }
        }

        private string _body;
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                if (_body == value) return;
                _body = value;
                OnPropertyChanged("Body");
            }
        }

        private string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                if (_subject == value) return;
                _subject = value;
                OnPropertyChanged("Subject");
            }
        }

        #endregion

        #region Commands

        private readonly RelayCommand _saveMessageCommand;
        public ICommand SaveMessageCommand { get { return _saveMessageCommand; } }

        #endregion

        readonly UsersContext _usersContext;
        private readonly MessageContext _messageContext;
        public event EventHandler ValidationErrorsEvent;

        public NewMessageViewModel()
        {
            _usersContext = ContextFactory.GetUserContext();
            _messageContext = ContextFactory.GetMessageContext();
            _saveMessageCommand = new RelayCommand(OnSaveMessage);
            UpdateForUsersRole();
            LoadData();
        }

        void OnSaveMessage()
        {
            if ( string.IsNullOrEmpty(To) )
            {
                ShowDialog( "Please select a recipient" );
                return;
            }

            if ( string.IsNullOrEmpty(From))
            {
                ShowDialog( "Please select a sender" );
                return;
            }

            if ( string.IsNullOrEmpty( Body ) )
            {
                ShowDialog( "Please fill in the body" );
                return;
            }

            if ( string.IsNullOrEmpty( Subject ) )
            {
                ShowDialog( "Please fill in the Subject" );
                return;
            }

            var msg = new Message
                          {
                              Body = Body, 
                              From = From,
                              To = To, 
                              Subject = Subject,
                              TimeStamp = DateTime.Now
                          };

            try
            {
                _messageContext.InsertMessage(msg).Completed += MessageFinishedInserting;
            }
            catch (Exception ex) { ShowDialog(ex.Message); }

            ShowDialog("The message has been successfully sent");
        }

        private static void MessageFinishedInserting(object sender, EventArgs e)
        {
            AppMessages.MailSentMessage.Send();
        }

        private void LoadData()
        {
            try
            {
                _usersContext.Load(_usersContext.GetAllUsersQuery()).Completed += UsersLoadCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;

            _saveMessageCommand.IsEnabled = isLoggedIn;
        }


        private void UsersLoadCompleted(object sender, EventArgs e)
        {
            var ent = ((LoadOperation<aspnet_User>)sender).Entities;

            Users = ent.AsEnumerable().Select(en => en.LoweredUserName);
        }

        public override void AuthenticationLoggedIn(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
        }

        public override void AuthenticationLoggedOut(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
        }
    }
}
