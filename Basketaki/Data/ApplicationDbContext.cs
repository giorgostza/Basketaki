using Microsoft.EntityFrameworkCore;
using Basketaki.Models;

namespace Basketaki.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchPhoto> MatchPhotos { get; set; }
        public DbSet<MatchReferee> MatchReferees { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerSeasonTeam> PlayerSeasonTeams { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamSeasonLeague> TeamSeasonLeagues { get; set; }
        public DbSet<TeamStanding> TeamStandings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);
            ConfigureConstraints(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Coach)
                .WithMany(c => c.Teams)
                .HasForeignKey(t => t.CoachId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.Court)
                .WithMany(c => c.Matches)
                .HasForeignKey(m => m.CourtId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.League)
                .WithMany(l => l.Matches)
                .HasForeignKey(m => m.LeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeamSeasonLeague)
                .WithMany(tsl => tsl.HomeMatches)
                .HasForeignKey(m => m.HomeTeamSeasonLeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeamSeasonLeague)
                .WithMany(tsl => tsl.AwayMatches)
                .HasForeignKey(m => m.AwayTeamSeasonLeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerStat>()
                .HasOne(ps => ps.PlayerSeasonTeam)
                .WithMany(pst => pst.PlayerStats)
                .HasForeignKey(ps => ps.PlayerSeasonTeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerStat>()
                .HasOne(ps => ps.Match)
                .WithMany(m => m.PlayerStats)
                .HasForeignKey(ps => ps.MatchId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MatchPhoto>()
                .HasOne(mp => mp.Match)
                .WithMany(m => m.Photos)
                .HasForeignKey(mp => mp.MatchId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MatchReferee>()
                .HasKey(mr => new { mr.MatchId, mr.RefereeId });


            modelBuilder.Entity<MatchReferee>()
                .HasOne(mr => mr.Match)
                .WithMany(m => m.MatchReferees)
                .HasForeignKey(mr => mr.MatchId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MatchReferee>()
                .HasOne(mr => mr.Referee)
                .WithMany(r => r.MatchReferees)
                .HasForeignKey(mr => mr.RefereeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TeamSeasonLeague>()
                .HasOne(tsl => tsl.Team)
                .WithMany(t => t.TeamSeasonLeagues)
                .HasForeignKey(tsl => tsl.TeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TeamSeasonLeague>()
                .HasOne(tsl => tsl.League)
                .WithMany(l => l.TeamSeasonLeagues)
                .HasForeignKey(tsl => tsl.LeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerSeasonTeam>()
                .HasOne(pst => pst.Player)
                .WithMany(p => p.PlayerSeasonTeams)
                .HasForeignKey(pst => pst.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerSeasonTeam>()
                .HasOne(pst => pst.Team)
                .WithMany(t => t.PlayerSeasonTeams)
                .HasForeignKey(pst => pst.TeamId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerSeasonTeam>()
                .HasOne(pst => pst.Season)
                .WithMany(s => s.PlayerSeasonTeams)
                .HasForeignKey(pst => pst.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<League>()
                .HasOne(l => l.Season)
                .WithMany(s => s.Leagues)
                .HasForeignKey(l => l.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TeamStanding>()
                .HasOne(ts => ts.TeamSeasonLeague)
                .WithOne()
                .HasForeignKey<TeamStanding>(ts => ts.TeamSeasonLeagueId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        private void ConfigureConstraints(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Season>().HasIndex(s => s.Name).IsUnique();

            modelBuilder.Entity<TeamSeasonLeague>().HasIndex(tsl => new { tsl.TeamId, tsl.LeagueId }).IsUnique();

            modelBuilder.Entity<PlayerSeasonTeam>().HasIndex(pst => new { pst.PlayerId, pst.TeamId, pst.SeasonId, pst.JoinDate }).IsUnique();

            modelBuilder.Entity<Match>().HasIndex(m => new { m.CourtId, m.MatchDate, m.StartTime }).IsUnique();

            modelBuilder.Entity<PlayerStat>().HasIndex(ps => new { ps.PlayerSeasonTeamId, ps.MatchId }).IsUnique();

            modelBuilder.Entity<Court>().HasIndex(c => new { c.Name, c.Location }).IsUnique();

            modelBuilder.Entity<League>().HasIndex(l => new { l.Name, l.City, l.SeasonId }).IsUnique();

            modelBuilder.Entity<Team>().HasIndex(t => new { t.Name, t.City }).IsUnique();

        }
    }
}