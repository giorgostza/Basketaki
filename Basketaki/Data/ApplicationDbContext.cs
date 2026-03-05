using Microsoft.EntityFrameworkCore;
using Basketaki.Models;

namespace Basketaki.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<Season> Seasons { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TeamSeasonLeague> TeamSeasonLeagues { get; set; }
        public DbSet<PlayerSeasonTeam> PlayerSeasonTeams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<Court> Courts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);
            ConfigureConstraints(modelBuilder);

        }


        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // =============================
            // Match - Home & Away Teams
            // =============================

            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeamSeasonLeague).WithMany().HasForeignKey(m => m.HomeTeamSeasonLeagueId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeamSeasonLeague).WithMany().HasForeignKey(m => m.AwayTeamSeasonLeagueId).OnDelete(DeleteBehavior.Restrict);
                
               

            // =============================
            // PlayerStat
            // =============================

            modelBuilder.Entity<PlayerStat>()
                .HasOne(ps => ps.PlayerSeasonTeam).WithMany().HasForeignKey(ps => ps.PlayerSeasonTeamId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlayerStat>()
                .HasOne(ps => ps.Match).WithMany(m => m.PlayerStats).HasForeignKey(ps => ps.MatchId).OnDelete(DeleteBehavior.Cascade);



        }


        private void ConfigureConstraints(ModelBuilder modelBuilder)
        {
            // ==========================================
            // 1️ Μια ομάδα -> 1 League ανά Season
            // ==========================================

            modelBuilder.Entity<TeamSeasonLeague>()
                .HasIndex(tsl => new { tsl.TeamId, tsl.LeagueId }).IsUnique();


            // ==========================================
            // 2️ Ένας παίκτης -> 1 ομάδα ανά Season
            // ==========================================

            modelBuilder.Entity<PlayerSeasonTeam>()
                .HasIndex(pst => new { pst.PlayerId, pst.SeasonId }).IsUnique();


            modelBuilder.Entity<Match>()
                .HasIndex(m => new { m.CourtId, m.MatchDate })
                .IsUnique();

        }

    }
}