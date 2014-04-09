
#region

using System;
using System.Data;
using System.Linq;
using System.ServiceModel.DomainServices.EntityFramework;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web.Security;
using Queryable = System.Linq.Queryable;

#endregion

namespace _1188.SCMS.Web.Services
{
    // Implements application logic using the SportsTeamEntities context.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess]
    public class TeamService : LinqToEntitiesDomainService<SportsTeamEntities>
    {
        #region leagues


        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Leagues' query.
        public System.Linq.IQueryable<League> GetLeagues()
        {
            return ObjectContext.Leagues;
        }

        [Query(IsComposable = false)]
        public League GetLeagueById(int leagueId)
        {
            var user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }

            return Queryable.FirstOrDefault(Queryable.Where(ObjectContext.Leagues, t => t.ID == leagueId));
        }

        [Update]
        public void UpdateLeague(League league)
        {
            ObjectContext.Leagues.AttachAsModified(league, ChangeSet.GetOriginal(league));
        }

        [Insert]
        public void InsertLeague(League league)
        {
            if ((league.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(league, EntityState.Added);
            }
            else
            {
                ObjectContext.Leagues.AddObject(league);
            }
        }

        [Delete]
        public void DeleteLeague(League league)
        {
            if ((league.EntityState == EntityState.Detached))
            {
                ObjectContext.Leagues.Attach(league);
            }
            ObjectContext.Leagues.DeleteObject(league);
        }

        #endregion


        public System.Linq.IQueryable<SportsSociety> GetSportsSocieties()
        {
            return ObjectContext.SportsSocieties;
        }


        #region TeamLeagues

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'TeamLeagues' query.
        public System.Linq.IQueryable<TeamLeague> GetTeamLeagues()
        {
            return ObjectContext.TeamLeagues.Include("Team").Include("League").Where(t=>t.Team.IsActive);
        }


        [Query(IsComposable = false)]
        public TeamLeague GetTeamLeagueById(int teamId, int leagueId, int year)
        {
            var user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }
            return Queryable.FirstOrDefault(Queryable.Where(ObjectContext.TeamLeagues, t => (t.TeamID == teamId && t.LeagueID == leagueId && t.Year == year)));
        }


        [Update]
        public void UpdateTeamLeague(TeamLeague currentTeamLeague)
        {
            ObjectContext.TeamLeagues.AttachAsModified(currentTeamLeague, ChangeSet.GetOriginal(currentTeamLeague));
        }

        [Insert]
        public void InsertTeamLeague(TeamLeague teamLeague)
        {
            if (GetTeamLeagueById(teamLeague.TeamID, teamLeague.LeagueID, teamLeague.Year) != null)
            {
                UpdateTeamLeague(teamLeague);
                return;
            }

            if ((teamLeague.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(teamLeague, EntityState.Added);
            }
            else
            {
                ObjectContext.TeamLeagues.AddObject(teamLeague);
            }
        }

        [Delete]
        public void DeleteTeamLeague(TeamLeague teamLeague)
        {
            if ((teamLeague.EntityState == EntityState.Detached))
            {
                ObjectContext.TeamLeagues.Attach(teamLeague);
            }
            ObjectContext.TeamLeagues.DeleteObject(teamLeague);
        }

        #endregion

        #region Teams


        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Teams' query.
        public System.Linq.IQueryable<Team> GetTeams()
        {
            return ObjectContext.Teams.Where(t=>t.IsActive);
        }

        public System.Linq.IQueryable<Team> GetTeamsByEvent(int eventId)
        {
            var originalEvent = ObjectContext.Events.Where(e => e.ID == eventId).First();
            
            var teamz = ObjectContext.EventTeams.Where(e => e.EventID == originalEvent.ID).Select(t=>t.Team);

            return teamz;
        }

        [Query(IsComposable = false)]
        public Team GetTeamById(int teamId)
        {
            MembershipUser user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }
            return Queryable.FirstOrDefault(Queryable.Where(ObjectContext.Teams, t => t.ID == teamId));
        }


        [Update]
        public void UpdateTeam(Team currentTeam)
        {
            ObjectContext.Teams.AttachAsModified(currentTeam, ChangeSet.GetOriginal(currentTeam));
        }

        [Insert]
        public void InsertTeam(Team team)
        {
            if ((team.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(team, EntityState.Added);
            }
            else
            {
                ObjectContext.Teams.AddObject(team);
            }
        }

        [Delete]
        public void DeleteTeam(Team team)
        {
            if ((team.EntityState == EntityState.Detached))
            {
                ObjectContext.Teams.Attach(team);
            }
            ObjectContext.Teams.DeleteObject(team);
            ObjectContext.SaveChanges();
        }

        #endregion

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'MemberTeams' query.
        public IQueryable<MemberTeam> GetMemberTeams()
        {
            return this.ObjectContext.MemberTeams.Include("Member").Include("Team");
        }

        public void InsertMemberTeam(MemberTeam memberTeam)
        {
            if ((memberTeam.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(memberTeam, EntityState.Added);
            }
            else
            {
                this.ObjectContext.MemberTeams.AddObject(memberTeam);
            }
        }

        public void UpdateMemberTeam(MemberTeam currentMemberTeam)
        {
            this.ObjectContext.MemberTeams.AttachAsModified(currentMemberTeam, this.ChangeSet.GetOriginal(currentMemberTeam));
        }

        public void DeleteMemberTeam(MemberTeam memberTeam)
        {
            if ((memberTeam.EntityState == EntityState.Detached))
            {
                this.ObjectContext.MemberTeams.Attach(memberTeam);
            }
            this.ObjectContext.MemberTeams.DeleteObject(memberTeam);
        }

        #region Member
        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Members' query.
        public IQueryable<Member> GetMembers()
        {
            SportsTeamEntities ent = new SportsTeamEntities();

            var items = ent.Members;
            return items;
        }

        [Query(IsComposable = false)]
        public Member GetMemberById(int id)
        {
            var user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }

            return ObjectContext.Members.Where(t => t.ID == id).FirstOrDefault();
        }

        [Query( IsComposable = false )]
        public Member GetMemberByName( string name )
        {
            var user = Membership.GetUser();
            if ( user == null )
            {
                throw new InvalidOperationException( "User not logged in" );
            }

            return ObjectContext.Members.Where( t => t.Name == name ).First();
        }

        [Insert]
        public void InsertMember(Member member)
        {
            if ((member.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(member, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Members.AddObject(member);
                this.ObjectContext.SaveChanges();
            }
        }

        //public void InsertAndSaveChanges(Member memberToInsert)
        //{
        //    this.ObjectContext.Members.AddObject(memberToInsert);
        //    this.ObjectContext.SaveChanges();
        //}

        [Update]
        public void UpdateMember(Member currentMember)
        {
            this.ObjectContext.Members.AttachAsModified(currentMember, this.ChangeSet.GetOriginal(currentMember));
        }

        public void DeleteMember(Member member)
        {
            if ((member.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Members.Attach(member);
            }
            this.ObjectContext.Members.DeleteObject(member);
        }
        #endregion
    }
}


