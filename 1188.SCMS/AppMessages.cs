using System;
using GalaSoft.MvvmLight.Messaging;

namespace _1188.SCMS
{
    /// <summary>
    /// class that defines all messages used in this application
    /// </summary>
    public static class AppMessages
    {
        enum MessageTypes
        {
            MailDeleted,
            MailFinishedDelete,
            MailSent,
            EventAddCompleted,
            CardAddCompleted,
            TicketAddedMessage,
            MemberValidationMessage,
            MemberAddedSuccessfully,
            NavigateToEditMemberMessage,
        }

        public static class MemberValidationMessage
        {
            public static void Send(string message)
            {
                Messenger.Default.Send(message, MessageTypes.MemberValidationMessage);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.MemberValidationMessage, action);
            }
        }

        public static class MemberAddedSuccessfullyMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.MemberAddedSuccessfully);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.MemberAddedSuccessfully, action);
            }
        }

        public static class MailDeletedMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.MailDeleted);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.MailDeleted, action);
            }
        }

        public static class MailFinishedDeletingMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.MailFinishedDelete);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.MailFinishedDelete, action);
            }
        }

        public static class MailSentMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.MailSent);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.MailSent, action);
            }
        }

        public static class EventAddedMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.EventAddCompleted);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.EventAddCompleted, action);
            }
        }

        public static class CardAddedMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.CardAddCompleted);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.CardAddCompleted, action);
            }
        }

        public static class TicketAddedMessage
        {
            public static void Send()
            {
                Messenger.Default.Send(string.Empty, MessageTypes.TicketAddedMessage);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.TicketAddedMessage, action);
            }
        }

        public static class NavigateToEditMemberMessage
        {
            public static void Send(string m)
            {
                Messenger.Default.Send( m, MessageTypes.NavigateToEditMemberMessage );
            }

            public static void Register( object recipient, Action<string> action )
            {
                Messenger.Default.Register( recipient, MessageTypes.NavigateToEditMemberMessage, action );
            }
        }
    }
}