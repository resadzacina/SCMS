using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;
using _1188.SCMS.Views;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class VisitorCardViewModel : ViewModelBase
    {
        private readonly RelayCommand _deleteVisitorCardCommand;
        public ICommand DeleteVisitorCardCommand { get { return _deleteVisitorCardCommand; } }

        private readonly RelayCommand _editCommand;
        public ICommand EditCommand { get { return _editCommand; } }

        private readonly VisitorCardContext _context;
        private readonly TeamContext _teamContext;

        //properties
        #region Properties
        private VisitorCard _selectedCard;
        public VisitorCard SelectedCard
        {
            get
            {
                return _selectedCard;
            }
            set
            {
                if ( _selectedCard == value ) return;

                _selectedCard = value;
                OnPropertyChanged( "SelectedCard" );
            }
        }

        private EntitySet<VisitorCard> _visitorCardList;
        public EntitySet<VisitorCard> VisitorCardList
        {
            get
            {
                return _visitorCardList;
            }
            set
            {
                if ( _visitorCardList == value ) return;
                _visitorCardList = value;
                OnPropertyChanged( "VisitorCardList" );
            }
        }

        private IEnumerable<Member> _memberList;
        public IEnumerable<Member> MemberList
        {
            get
            {
                return _memberList;
            }
            set
            {
                if ( _memberList != value )
                {
                    _memberList = value;
                    OnPropertyChanged( "MemberList" );
                }
            }
        }

        #endregion

        public VisitorCardViewModel()
        {
            _context = ContextFactory.GetVisitorCardContext();
            _teamContext = ContextFactory.GetTeamContext();
            IsLoggedIn = true;
            MemberList = _teamContext.Members;
            VisitorCardList = _context.VisitorCards;

            AppMessages.CardAddedMessage.Register( this, OnCardAdded );

            _deleteVisitorCardCommand = new RelayCommand( OnDelete );
            _editCommand = new RelayCommand( OnEdit );
            LoadData();
        }

        private void OnCardAdded( string obj )
        {
            LoadData();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            _context.Load( _context.GetVisitorCardsQuery() ).Completed += OnCardsLoadCompleted;
            _teamContext.Load( _teamContext.GetMembersQuery() );
        }

        private void OnDelete()
        {
            if ( SelectedCard == null )
            {
                ShowDialog( "You must select a visitor card in order to delete it" );
                return;
            }

            _context.VisitorCards.Remove( SelectedCard );
            _context.SubmitChanges();
        }

        private void OnEdit()
        {
            var selected = SelectedCard;
            if ( selected == null )
            {
                ShowDialog( "You must select a visitor card in order to edit it" );
                return;
            }

            var id = selected.ID;
            var dialog = new EditVisitorCardView( id );
            dialog.Show();
        }

        private void OnCardsLoadCompleted( object sender, EventArgs e )
        {
            var count = VisitorCardList.Count();

            _deleteVisitorCardCommand.IsEnabled = count > 0;
            _editCommand.IsEnabled = count > 0;
        }

        public override void AuthenticationLoggedIn( object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e )
        {
        }

        public override void AuthenticationLoggedOut( object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e )
        {
        }
    }
}
