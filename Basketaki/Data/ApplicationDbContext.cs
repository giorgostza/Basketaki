using Microsoft.EntityFrameworkCore;
using Basketaki.Models;

namespace Basketaki.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);



            ConfigureRelationships(modelBuilder);
            ConfigureConstraints(modelBuilder);

        }



        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // One Court has a lot of Matches (1-many)

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
                .WithMany()
                .HasForeignKey(m => m.HomeTeamSeasonLeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeamSeasonLeague)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamSeasonLeagueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerStat>()
                .HasOne(ps => ps.PlayerSeasonTeam)
                .WithMany()
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
                .HasOne(mr => mr.Match)
                .WithMany()
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
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<TeamSeasonLeague>()
                .HasOne(tsl => tsl.League)
                .WithMany(l => l.TeamSeasonLeagues)
                .HasForeignKey(tsl => tsl.LeagueId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PlayerSeasonTeam>()
                .HasOne(pst => pst.Player)
                .WithMany(p => p.PlayerSeasonTeams)
                .HasForeignKey(pst => pst.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);


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
        }



        private void ConfigureConstraints(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<TeamSeasonLeague>().HasIndex(tsl => new { tsl.TeamId, tsl.LeagueId }).IsUnique();  // One Team cant join in the same league more than one time


            modelBuilder.Entity<PlayerSeasonTeam>().HasIndex(pst => new { pst.PlayerId, pst.SeasonId }).IsUnique(); // One Player can participate only to one Team in the same Season 


            modelBuilder.Entity<Match>().HasIndex(m => new { m.CourtId, m.MatchDate }).IsUnique();  // One Court cant have two games at the same time.


            modelBuilder.Entity<PlayerStat>().HasIndex(ps => new { ps.PlayerSeasonTeamId, ps.MatchId }).IsUnique();  // One Player Stat per game 
            

            modelBuilder.Entity<MatchReferee>().HasIndex(mr => new { mr.MatchId, mr.RefereeId }).IsUnique();  // One Referee cant be assigned twice in the same Match


        }
    }
}