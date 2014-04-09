using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Web.Security;

namespace _1188.SCMS.Web.Services
{
    [EnableClientAccess(RequiresSecureEndpoint = false /* This should be set to true before the application is deployed */)]
    public class MembershipService : DomainService
    {
        [RequiresRole("Administrator")]
        public IEnumerable<MembershipServiceUser> GetUsers()
        {
            return Membership.GetAllUsers().Cast<MembershipUser>().Select(u => new MembershipServiceUser(u));
        }

        [RequiresRole("Administrator")]
        public IEnumerable<MembershipServiceUser> GetUsersByEmail(string email)
        {
            return Membership.FindUsersByEmail(email).Cast<MembershipUser>().Select(u => new MembershipServiceUser(u));
        }

        [RequiresRole("Administrator")]
        public IEnumerable<MembershipServiceUser> GetUsersByName(string userName)
        {
            return Membership.FindUsersByName(userName).Cast<MembershipUser>().Select(u => new MembershipServiceUser(u));
        }

        [Invoke(HasSideEffects = true)]
        public void CreateUser(MembershipServiceUser user, string password)
        {
            Membership.CreateUser(user.UserName, password, user.Email);
        }

        [RequiresRole("Administrator")]
        public void DeleteUser(MembershipServiceUser user)
        {
            Membership.DeleteUser(user.UserName);
        }

        [RequiresRole("Administrator")]
        public void UpdateUser(MembershipServiceUser user)
        {
            Membership.UpdateUser(user.ToMembershipUser());
        }


       [Update(UsingCustomMethod = true)]
        public void ChangePassword(MembershipServiceUser user, string oldPassword, string newPassword)
        {
            user.ToMembershipUser().ChangePassword(oldPassword, newPassword);
        }

        [RequiresRole("Administrator")]
        public void ResetPassword(MembershipServiceUser user)
        {
            user.ToMembershipUser().ResetPassword();
        }

        [RequiresRole("Administrator")]
        public void UnlockUser(MembershipServiceUser user)
        {
            user.ToMembershipUser().UnlockUser();
        }
    }

    public class MembershipServiceUser
    {
        public string Comment { get; set; }
        [Editable(false)]
        public DateTime CreationDate { get; set; }
        [Key]
        [Editable(false, AllowInitialValue = true)]
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        [Editable(false)]
        public bool IsLockedOut { get; set; }
        [Editable(false)]
        public bool IsOnline { get; set; }
        public DateTime LastActivityDate { get; set; }
        [Editable(false)]
        public DateTime LastLockoutDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        [Editable(false)]
        public DateTime LastPasswordChangedDate { get; set; }
        [Editable(false)]
        public string PasswordQuestion { get; set; }
        [Key]
        [Editable(false, AllowInitialValue = true)]
        public string UserName { get; set; }

        public MembershipServiceUser() { }
        public MembershipServiceUser(MembershipUser user)
        {
            this.FromMembershipUser(user);
        }

        public void FromMembershipUser(MembershipUser user)
        {
            this.Comment = user.Comment;
            this.CreationDate = user.CreationDate;
            this.Email = user.Email;
            this.IsApproved = user.IsApproved;
            this.IsLockedOut = user.IsLockedOut;
            this.IsOnline = user.IsOnline;
            this.LastActivityDate = user.LastActivityDate;
            this.LastLockoutDate = user.LastLockoutDate;
            this.LastLoginDate = user.LastLoginDate;
            this.LastPasswordChangedDate = user.LastPasswordChangedDate;
            this.PasswordQuestion = user.PasswordQuestion;
            this.UserName = user.UserName;
        }

        public MembershipUser ToMembershipUser()
        {
            MembershipUser user = Membership.GetUser(this.UserName);

            if (user.Comment != this.Comment) user.Comment = this.Comment;
            if (user.IsApproved != this.IsApproved) user.IsApproved = this.IsApproved;
            if (user.LastActivityDate != this.LastActivityDate) user.LastActivityDate = this.LastActivityDate;
            if (user.LastLoginDate != this.LastLoginDate) user.LastLoginDate = this.LastLoginDate;

            return user;
        }
    }

}