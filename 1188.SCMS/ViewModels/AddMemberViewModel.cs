using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using _1188.SCMS.Models;
using _1188.SCMS.Web;

namespace _1188.SCMS.ViewModels
{
    public class AddMemberViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        private readonly UserRegistrationContext _userContext;

        #region Properties

        //private ValidationSummary _errorItems;
        //public ValidationSummary ErrorItems
        //{
        //    get
        //    {
        //        return _errorItems;
        //    }
        //    set
        //    {
        //        _errorItems = value;
        //        OnPropertyChanged("ErrorItems");
        //    }
        //}

        private NewUserData _userToInsert;
        public NewUserData UserToInsert
        {
            get
            {
                return _userToInsert;
            }
            set
            {
                _userToInsert = value;
                OnPropertyChanged("UserToInsert");
            }
        }

        private Visibility _isStatusVisible;
        public Visibility IsStatusVisible
        {
            get
            {
                return _isStatusVisible;
            }
            set
            {
                _isStatusVisible = value;
                OnPropertyChanged("IsStatusVisible");
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        #endregion

        public AddMemberViewModel()
        {
            _userContext = ContextFactory.GetUserRegistrationContext();
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            UserToInsert = new NewUserData();
            IsLoggedIn = true;
            UpdateForUsersRole();
        }

        void OnSaveChanges()
        {
            var data = new RegistrationData();

            try
            {
                data.Answer = UserToInsert.Answer;
                data.Email = UserToInsert.Email;
                data.FriendlyName = UserToInsert.FriendlyName;
                data.Password = UserToInsert.Password;
                data.PasswordConfirmation = UserToInsert.PasswordConfirmation;
                data.Question = UserToInsert.Question;
                data.UserName = UserToInsert.UserName;


                Validator.ValidateProperty(UserToInsert.UserName,
                   new ValidationContext(data, null, null) { MemberName = "UserName" });

                Validator.ValidateProperty(UserToInsert.Email,
                 new ValidationContext(data, null, null) { MemberName = "Email" });

                Validator.ValidateProperty(UserToInsert.Password,
                new ValidationContext(data, null, null) { MemberName = "Password" });

                Validator.ValidateProperty(UserToInsert.PasswordConfirmation,
                new ValidationContext(data, null, null) { MemberName = "PasswordConfirmation" });

                Validator.ValidateProperty(UserToInsert.Question,
               new ValidationContext(data, null, null) { MemberName = "Question" });

                Validator.ValidateProperty(UserToInsert.Answer,
             new ValidationContext(data, null, null) { MemberName = "Answer" });

                if (UserToInsert.Password != UserToInsert.PasswordConfirmation)
                {
                    throw new ValidationException("Password and confirmation are not the same");
                }
            }
            catch (ValidationException ex)
            {
                var member = ((string[])(ex.ValidationResult.MemberNames)).Count() > 0
                                  ? ((string[]) (ex.ValidationResult.MemberNames))[0]
                                  : string.Empty;

                AppMessages.MemberValidationMessage.Send(member + " - " + ex.Message);
                return;
            }

            _userContext.CreateUser(data, UserToInsert.Password, CreationOfMemberCompleted, null);
        }

        private void CreationOfMemberCompleted(InvokeOperation<CreateUserStatus> invOp)
        {
            string message;

            if (invOp.Value == CreateUserStatus.Success)
            {
                message = "New Member has been successfully added";
                AppMessages.MemberAddedSuccessfullyMessage.Send();
            }
            else
                message = "Adding new member has failed with following message: " + invOp.Value;

            ShowDialog(message);
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;
            UpdateButtons(isLoggedIn);
        }

        private void UpdateButtons(bool isLoggedIn)
        {
            _saveChangesCommand.IsEnabled = isLoggedIn;
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = true;
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = false;
        }
    }
}
