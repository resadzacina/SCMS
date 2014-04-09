
namespace _1188.SCMS.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies LeagueMetadata as the class
    // that carries additional metadata for the League class.
    [MetadataTypeAttribute(typeof(League.LeagueMetadata))]
    public partial class League
    {

        // This class allows you to attach custom attributes to properties
        // of the League class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class LeagueMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private LeagueMetadata()
            {
            }

            public int ID { get; set; }

            [StringLength(70, MinimumLength = 3)]
            [Required(AllowEmptyStrings = false)]
            public string Name { get; set; }

            public EntityCollection<TeamLeague> TeamLeagues { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SportsSocietyMetadata as the class
    // that carries additional metadata for the SportsSociety class.
    [MetadataTypeAttribute(typeof(SportsSociety.SportsSocietyMetadata))]
    public partial class SportsSociety
    {

        // This class allows you to attach custom attributes to properties
        // of the SportsSociety class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SportsSocietyMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SportsSocietyMetadata()
            {
            }

            public EntityCollection<Building> Buildings { get; set; }

            public int ID { get; set; }

            public string Name { get; set; }

            public EntityCollection<Team> Teams { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TeamMetadata as the class
    // that carries additional metadata for the Team class.
    [MetadataTypeAttribute(typeof(Team.TeamMetadata))]
    public partial class Team
    {

        // This class allows you to attach custom attributes to properties
        // of the Team class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TeamMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TeamMetadata()
            {
            }

            public string Address { get; set; }

            public string City { get; set; }

            public EntityCollection<EventTeam> EventTeams { get; set; }

            public int ID { get; set; }

            [StringLength(70, MinimumLength = 3)]
            [Required]
            public string Name { get; set; }

            public string Phone { get; set; }

            public Nullable<int> PlayersCount { get; set; }

            [Required]
            public int SportSocietyID { get; set; }

            [Include]
            public SportsSociety SportsSociety { get; set; }

            public string SportType { get; set; }

            public EntityCollection<TeamLeague> TeamLeagues { get; set; }

            public string Website { get; set; }

            public Nullable<short> YearFounded { get; set; }

            public bool IsActive {get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TeamLeagueMetadata as the class
    // that carries additional metadata for the TeamLeague class.
    [MetadataTypeAttribute(typeof(TeamLeague.TeamLeagueMetadata))]
    public partial class TeamLeague
    {

        // This class allows you to attach custom attributes to properties
        // of the TeamLeague class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TeamLeagueMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TeamLeagueMetadata()
            {
            }

            [Include]
            public League League { get; set; }

            public int LeagueID { get; set; }

            [Include]
            public Team Team { get; set; }

            public int TeamID { get; set; }

            public short Year { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies MemberTeamMetadata as the class
    // that carries additional metadata for the MemberTeam class.
    [MetadataTypeAttribute(typeof(MemberTeam.MemberTeamMetadata))]
    public partial class MemberTeam
    {

        // This class allows you to attach custom attributes to properties
        // of the MemberTeam class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class MemberTeamMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private MemberTeamMetadata()
            {
            }

            [Include]
            public Member Member { get; set; }

            public Guid MemberID { get; set; }

            [Include]
            public Team Team { get; set; }

            public int TeamID { get; set; }

            public int Year { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies MemberMetadata as the class
    // that carries additional metadata for the Member class.
    [MetadataTypeAttribute(typeof(Member.MemberMetadata))]
    public partial class Member
    {

        // This class allows you to attach custom attributes to properties
        // of the Member class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class MemberMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private MemberMetadata()
            {
            }

            public string Address1 { get; set; }

            public string Address2 { get; set; }

            public aspnet_User aspnet_Users { get; set; }

            public Nullable<DateTime> Birthdate { get; set; }

            public string City { get; set; }

            public string Comments { get; set; }

            public Country Country { get; set; }

            public Nullable<int> CountryID { get; set; }

            public string Gender { get; set; }

            public string HomePhone { get; set; }

            [RoundtripOriginal]
            public int ID { get; set; }

            public string Mail { get; set; }

            public string MobilePhone { get; set; }

            public string Name { get; set; }

            public string Nationality { get; set; }

            public string Nickname { get; set; }

            public byte[] Photo { get; set; }

            public Nullable<int> PostalCode { get; set; }

            public string Surname { get; set; }

            public EntityCollection<MemberTeam> MemberTeams { get; set; }

            public Guid UserID { get; set; }

            public string WorkPhone { get; set; }
        }
    }
}
