using Basketaki.Constants;
using Basketaki.Models;

namespace Basketaki.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Seasons.Any())
            {
                return;
            }


           
            var season1 = new Season
            {
                Name = "2025-2026",
                StartDate = new DateOnly(2025, 9, 1),
                EndDate = new DateOnly(2026, 6, 30)
            };

            var season2 = new Season
            {
                Name = "2026-2027",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2027, 6, 30)
            };

            context.Seasons.AddRange(season1, season2);
            context.SaveChanges();


           
            var coach1 = new Coach
            {
                FirstName = "Nikos",
                LastName = "Papadakis",
                DateOfBirth = new DateOnly(1980, 5, 10),
                Height = 182,
                PhotoUrl = "/images/coaches/coach1.jpg"
            };

            var coach2 = new Coach
            {
                FirstName = "Giorgos",
                LastName = "Nikolaou",
                DateOfBirth = new DateOnly(1978, 3, 22),
                Height = 185,
                PhotoUrl = "/images/coaches/coach2.jpg"
            };

            var coach3 = new Coach
            {
                FirstName = "Manolis",
                LastName = "Karalis",
                DateOfBirth = new DateOnly(1983, 9, 14),
                Height = 181,
                PhotoUrl = "/images/coaches/coach3.jpg"
            };

            context.Coaches.AddRange(coach1, coach2, coach3);
            context.SaveChanges();


            
            var court1 = new Court
            {
                Name = "Basketaki Arena",
                Location = "Menidi",
                Description = "Indoor court"
            };

            var court2 = new Court
            {
                Name = "Elite Court",
                Location = "Peristeri",
                Description = "Outdoor court"
            };

            var court3 = new Court
            {
                Name = "Downtown Hoops",
                Location = "Athens Center",
                Description = "Indoor court with scoreboard"
            };

            context.Courts.AddRange(court1, court2, court3);
            context.SaveChanges();

            

            var referee1 = new Referee
            {
                FirstName = "Dimitris",
                LastName = "Ioannou",
                DateOfBirth = new DateOnly(1988, 6, 12),
                Height = 180,
                PhotoUrl = "/images/referees/ref1.jpg"
            };

            var referee2 = new Referee
            {
                FirstName = "Spyros",
                LastName = "Georgiou",
                DateOfBirth = new DateOnly(1985, 11, 3),
                Height = 178,
                PhotoUrl = "/images/referees/ref2.jpg"
            };

            var referee3 = new Referee
            {
                FirstName = "Vasilis",
                LastName = "Antonakis",
                DateOfBirth = new DateOnly(1990, 2, 18),
                Height = 183,
                PhotoUrl = "/images/referees/ref3.jpg"
            };

            context.Referees.AddRange(referee1, referee2, referee3);
            context.SaveChanges();

            

            var player1 = new Player
            {
                FirstName = "Giannis",
                LastName = "Tzavellas",
                DateOfBirth = new DateOnly(1995, 4, 15),
                Height = 191,
                Weight = 92,
                Position = PlayerPosition.Center,
                PhotoUrl = "/images/players/p1.jpg"
            };

            var player2 = new Player
            {
                FirstName = "Kostas",
                LastName = "Milonas",
                DateOfBirth = new DateOnly(1998, 8, 20),
                Height = 186,
                Weight = 85,
                Position = PlayerPosition.Point_Guard,
                PhotoUrl = "/images/players/p2.jpg"
            };

            var player3 = new Player
            {
                FirstName = "Petros",
                LastName = "Lazarou",
                DateOfBirth = new DateOnly(1997, 1, 9),
                Height = 194,
                Weight = 88,
                Position = PlayerPosition.Power_Forward,
                PhotoUrl = "/images/players/p3.jpg"
            };

            var player4 = new Player
            {
                FirstName = "Nikos",
                LastName = "Bellas",
                DateOfBirth = new DateOnly(1999, 12, 1),
                Height = 183,
                Weight = 80,
                Position = PlayerPosition.Shooting_Guard,
                PhotoUrl = "/images/players/p4.jpg"
            };

            var player5 = new Player
            {
                FirstName = "Andreas",
                LastName = "Kotsis",
                DateOfBirth = new DateOnly(1996, 7, 7),
                Height = 188,
                Weight = 84,
                Position = PlayerPosition.Small_Forward,
                PhotoUrl = "/images/players/p5.jpg"
            };

            var player6 = new Player
            {
                FirstName = "Marios",
                LastName = "Raptis",
                DateOfBirth = new DateOnly(1994, 3, 30),
                Height = 196,
                Weight = 95,
                Position = PlayerPosition.Center,
                PhotoUrl = "/images/players/p6.jpg"
            };

            var player7 = new Player
            {
                FirstName = "Theo",
                LastName = "Mpakas",
                DateOfBirth = new DateOnly(2000, 5, 5),
                Height = 182,
                Weight = 79,
                Position = PlayerPosition.Point_Guard,
                PhotoUrl = "/images/players/p7.jpg"
            };

            var player8 = new Player
            {
                FirstName = "Aris",
                LastName = "Drosos",
                DateOfBirth = new DateOnly(1998, 10, 11),
                Height = 190,
                Weight = 86,
                Position = PlayerPosition.Small_Forward,
                PhotoUrl = "/images/players/p8.jpg"
            };

            context.Players.AddRange(player1, player2, player3, player4, player5, player6, player7, player8);
            context.SaveChanges();

            

            var team1 = new Team
            {
                Name = "Gran Camaria",
                City = "Athens",
                PhotoUrl = "/images/teams/team1.png",
                CoachId = coach1.Id
            };

            var team2 = new Team
            {
                Name = "Rejects",
                City = "Piraeus",
                PhotoUrl = "/images/teams/team2.png",
                CoachId = coach2.Id
            };

            var team3 = new Team
            {
                Name = "Street Kings",
                City = "Athens",
                PhotoUrl = "/images/teams/team3.png",
                CoachId = coach3.Id
            };

            var team4 = new Team
            {
                Name = "West Ballers",
                City = "Peristeri",
                PhotoUrl = "/images/teams/team4.png",
                CoachId = null
            };

            context.Teams.AddRange(team1, team2, team3, team4);
            context.SaveChanges();

           

            var league1 = new League
            {
                Name = "Master League",
                City = "Athens",
                SeasonId = season1.Id
            };

            var league2 = new League
            {
                Name = "Urban League",
                City = "Piraeus",
                SeasonId = season1.Id
            };

            var league3 = new League
            {
                Name = "Master League",
                City = "Athens",
                SeasonId = season2.Id
            };

            context.Leagues.AddRange(league1, league2, league3);
            context.SaveChanges();

            

            var tsl1 = new TeamSeasonLeague
            {
                TeamId = team1.Id,
                LeagueId = league1.Id
            };

            var tsl2 = new TeamSeasonLeague
            {
                TeamId = team2.Id,
                LeagueId = league1.Id
            };

            var tsl3 = new TeamSeasonLeague
            {
                TeamId = team3.Id,
                LeagueId = league1.Id
            };

            var tsl4 = new TeamSeasonLeague
            {
                TeamId = team4.Id,
                LeagueId = league2.Id
            };

            var tsl5 = new TeamSeasonLeague
            {
                TeamId = team2.Id,
                LeagueId = league2.Id
            };

            var tsl6 = new TeamSeasonLeague
            {
                TeamId = team1.Id,
                LeagueId = league3.Id
            };

            context.TeamSeasonLeagues.AddRange(tsl1, tsl2, tsl3, tsl4, tsl5, tsl6);
            context.SaveChanges();

           

            var pst1 = new PlayerSeasonTeam
            {
                PlayerId = player1.Id,
                TeamId = team1.Id,
                SeasonId = season1.Id,
                JerseyNumber = 15,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst2 = new PlayerSeasonTeam
            {
                PlayerId = player2.Id,
                TeamId = team1.Id,
                SeasonId = season1.Id,
                JerseyNumber = 7,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst3 = new PlayerSeasonTeam
            {
                PlayerId = player3.Id,
                TeamId = team2.Id,
                SeasonId = season1.Id,
                JerseyNumber = 9,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst4 = new PlayerSeasonTeam
            {
                PlayerId = player4.Id,
                TeamId = team2.Id,
                SeasonId = season1.Id,
                JerseyNumber = 11,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst5 = new PlayerSeasonTeam
            {
                PlayerId = player5.Id,
                TeamId = team3.Id,
                SeasonId = season1.Id,
                JerseyNumber = 10,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst6 = new PlayerSeasonTeam
            {
                PlayerId = player6.Id,
                TeamId = team3.Id,
                SeasonId = season1.Id,
                JerseyNumber = 14,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst7 = new PlayerSeasonTeam
            {
                PlayerId = player7.Id,
                TeamId = team4.Id,
                SeasonId = season1.Id,
                JerseyNumber = 4,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst8 = new PlayerSeasonTeam
            {
                PlayerId = player8.Id,
                TeamId = team4.Id,
                SeasonId = season1.Id,
                JerseyNumber = 8,
                JoinDate = new DateOnly(2025, 9, 1)
            };

            var pst9 = new PlayerSeasonTeam
            {
                PlayerId = player1.Id,
                TeamId = team1.Id,
                SeasonId = season2.Id,
                JerseyNumber = 15,
                JoinDate = new DateOnly(2026, 9, 1)
            };

            context.PlayerSeasonTeams.AddRange(pst1, pst2, pst3, pst4, pst5, pst6, pst7, pst8, pst9);
            context.SaveChanges();

            

            var match1 = new Match
            {
                MatchDate = new DateOnly(2025, 10, 10),
                StartTime = new TimeOnly(18, 0),
                EndTime = new TimeOnly(19, 30),
                CourtId = court1.Id,
                LeagueId = league1.Id,
                HomeTeamSeasonLeagueId = tsl1.Id,
                AwayTeamSeasonLeagueId = tsl2.Id,
                HomeScore = 75,
                AwayScore = 69,
                IsPlayed = true
            };

            var match2 = new Match
            {
                MatchDate = new DateOnly(2025, 10, 17),
                StartTime = new TimeOnly(20, 0),
                EndTime = new TimeOnly(21, 30),
                CourtId = court3.Id,
                LeagueId = league1.Id,
                HomeTeamSeasonLeagueId = tsl3.Id,
                AwayTeamSeasonLeagueId = tsl1.Id,
                HomeScore = 71,
                AwayScore = 78,
                IsPlayed = true
            };

            var match3 = new Match
            {
                MatchDate = new DateOnly(2025, 11, 2),
                StartTime = new TimeOnly(19, 0),
                EndTime = new TimeOnly(20, 30),
                CourtId = court2.Id,
                LeagueId = league2.Id,
                HomeTeamSeasonLeagueId = tsl4.Id,
                AwayTeamSeasonLeagueId = tsl5.Id,
                IsPlayed = false
            };

            context.Matches.AddRange(match1, match2, match3);
            context.SaveChanges();

            

            var photo1 = new MatchPhoto
            {
                ImageUrl = "/images/matches/match1_photo1.jpg",
                MatchId = match1.Id
            };

            var photo2 = new MatchPhoto
            {
                ImageUrl = "/images/matches/match1_photo2.jpg",
                MatchId = match1.Id
            };

            var photo3 = new MatchPhoto
            {
                ImageUrl = "/images/matches/match2_photo1.jpg",
                MatchId = match2.Id
            };

            context.MatchPhotos.AddRange(photo1, photo2, photo3);
            context.SaveChanges();

           

            var mr1 = new MatchReferee
            {
                MatchId = match1.Id,
                RefereeId = referee1.Id
            };

            var mr2 = new MatchReferee
            {
                MatchId = match1.Id,
                RefereeId = referee2.Id
            };

            var mr3 = new MatchReferee
            {
                MatchId = match2.Id,
                RefereeId = referee2.Id
            };

            var mr4 = new MatchReferee
            {
                MatchId = match2.Id,
                RefereeId = referee3.Id
            };

            context.MatchReferees.AddRange(mr1, mr2, mr3, mr4);
            context.SaveChanges();

            

            var stat1 = new PlayerStat
            {
                PlayerSeasonTeamId = pst1.Id,
                MatchId = match1.Id,
                Points = 19,
                OffensiveRebounds = 3,
                DefensiveRebounds = 8,
                Assists = 4,
                Steals = 1,
                Blocks = 2,
                Fouls = 3,
                MinutesPlayed = 32,
                FreeThrowsMade = 3,
                FreeThrowsAttempted = 4,
                TwoPointsMade = 8,
                TwoPointsAttempted = 12,
                ThreePointsMade = 0,
                ThreePointsAttempted = 1,
                IsMVP = true,
                SuspensionGames = 0
            };

            var stat2 = new PlayerStat
            {
                PlayerSeasonTeamId = pst2.Id,
                MatchId = match1.Id,
                Points = 14,
                OffensiveRebounds = 1,
                DefensiveRebounds = 4,
                Assists = 6,
                Steals = 2,
                Blocks = 0,
                Fouls = 2,
                MinutesPlayed = 30,
                FreeThrowsMade = 2,
                FreeThrowsAttempted = 2,
                TwoPointsMade = 3,
                TwoPointsAttempted = 6,
                ThreePointsMade = 2,
                ThreePointsAttempted = 5,
                IsMVP = false,
                SuspensionGames = 0
            };

            var stat3 = new PlayerStat
            {
                PlayerSeasonTeamId = pst3.Id,
                MatchId = match1.Id,
                Points = 17,
                OffensiveRebounds = 2,
                DefensiveRebounds = 6,
                Assists = 3,
                Steals = 2,
                Blocks = 1,
                Fouls = 2,
                MinutesPlayed = 30,
                FreeThrowsMade = 4,
                FreeThrowsAttempted = 5,
                TwoPointsMade = 5,
                TwoPointsAttempted = 10,
                ThreePointsMade = 1,
                ThreePointsAttempted = 3,
                IsMVP = false,
                SuspensionGames = 0
            };

            var stat4 = new PlayerStat
            {
                PlayerSeasonTeamId = pst5.Id,
                MatchId = match2.Id,
                Points = 21,
                OffensiveRebounds = 2,
                DefensiveRebounds = 5,
                Assists = 4,
                Steals = 1,
                Blocks = 1,
                Fouls = 3,
                MinutesPlayed = 34,
                FreeThrowsMade = 5,
                FreeThrowsAttempted = 6,
                TwoPointsMade = 5,
                TwoPointsAttempted = 9,
                ThreePointsMade = 2,
                ThreePointsAttempted = 6,
                IsMVP = false,
                SuspensionGames = 0
            };

            var stat5 = new PlayerStat
            {
                PlayerSeasonTeamId = pst6.Id,
                MatchId = match2.Id,
                Points = 12,
                OffensiveRebounds = 4,
                DefensiveRebounds = 7,
                Assists = 1,
                Steals = 0,
                Blocks = 2,
                Fouls = 4,
                MinutesPlayed = 28,
                FreeThrowsMade = 2,
                FreeThrowsAttempted = 3,
                TwoPointsMade = 5,
                TwoPointsAttempted = 8,
                ThreePointsMade = 0,
                ThreePointsAttempted = 1,
                IsMVP = false,
                SuspensionGames = 0
            };

            var stat6 = new PlayerStat
            {
                PlayerSeasonTeamId = pst1.Id,
                MatchId = match2.Id,
                Points = 24,
                OffensiveRebounds = 3,
                DefensiveRebounds = 9,
                Assists = 3,
                Steals = 2,
                Blocks = 1,
                Fouls = 2,
                MinutesPlayed = 35,
                FreeThrowsMade = 6,
                FreeThrowsAttempted = 7,
                TwoPointsMade = 8,
                TwoPointsAttempted = 13,
                ThreePointsMade = 0,
                ThreePointsAttempted = 1,
                IsMVP = true,
                SuspensionGames = 0
            };

            context.PlayerStats.AddRange(stat1, stat2, stat3, stat4, stat5, stat6);
            context.SaveChanges();

           

            var standing1 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl1.Id,
                Played = 2,
                Wins = 2,
                Losses = 0,
                PointsFor = 153,
                PointsAgainst = 140,
                LeaguePoints = 4,
                NoShow = 0,
                CurrentStreak = 2
            };

            var standing2 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl2.Id,
                Played = 1,
                Wins = 0,
                Losses = 1,
                PointsFor = 69,
                PointsAgainst = 75,
                LeaguePoints = 1,
                NoShow = 0,
                CurrentStreak = -1
            };

            var standing3 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl3.Id,
                Played = 1,
                Wins = 0,
                Losses = 1,
                PointsFor = 71,
                PointsAgainst = 78,
                LeaguePoints = 1,
                NoShow = 0,
                CurrentStreak = -1
            };

            var standing4 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl4.Id,
                Played = 0,
                Wins = 0,
                Losses = 0,
                PointsFor = 0,
                PointsAgainst = 0,
                LeaguePoints = 0,
                NoShow = 0,
                CurrentStreak = 0
            };

            var standing5 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl5.Id,
                Played = 0,
                Wins = 0,
                Losses = 0,
                PointsFor = 0,
                PointsAgainst = 0,
                LeaguePoints = 0,
                NoShow = 0,
                CurrentStreak = 0
            };

            var standing6 = new TeamStanding
            {
                TeamSeasonLeagueId = tsl6.Id,
                Played = 0,
                Wins = 0,
                Losses = 0,
                PointsFor = 0,
                PointsAgainst = 0,
                LeaguePoints = 0,
                NoShow = 0,
                CurrentStreak = 0
            };

            context.TeamStandings.AddRange(standing1, standing2, standing3, standing4, standing5, standing6);
            context.SaveChanges();

        }
    }
}