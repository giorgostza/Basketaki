using Basketaki.Dtos;
using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandingsApiController : ControllerBase
    {

        private readonly ITeamStandingService _teamStandingService;

        public StandingsApiController(ITeamStandingService teamStandingService)
        {
            _teamStandingService = teamStandingService;
        }



        // GET: api/standings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamStandingDto>>> GetAll()
        {
            var standings = await _teamStandingService.GetAllAsync();

            var result = standings.Select(MapToDto);

            return Ok(result);

        }




        // GET: api/standings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamStandingDto>> GetById(int id)
        {
            var standing = await _teamStandingService.GetByIdAsync(id);

            if (standing == null)
            {

                return NotFound();

            }


            return Ok(MapToDto(standing));

        }




        // GET: api/standings/by-league/3
        [HttpGet("by-league/{leagueId}")]
        public async Task<ActionResult<IEnumerable<TeamStandingDto>>> GetByLeague(int leagueId)
        {

            var standings = await _teamStandingService.GetAllAsync();

            var result = standings.Where(s => s.TeamSeasonLeague.LeagueId == leagueId).Select(MapToDto);


            return Ok(result);

        }




        // POST: api/standings
        [HttpPost]
        public async Task<ActionResult> Create(TeamStandingCreateUpdateDto dto)
        {
            var standing = new TeamStanding
            {

                TeamSeasonLeagueId = dto.TeamSeasonLeagueId,
                Played = dto.Played,
                Wins = dto.Wins,
                Losses = dto.Losses,
                PointsFor = dto.PointsFor,
                PointsAgainst = dto.PointsAgainst,
                LeaguePoints = dto.LeaguePoints,
                NoShow = dto.NoShow,
                CurrentStreak = dto.CurrentStreak

            };


            var result = await _teamStandingService.CreateAsync(standing);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }



            return Ok(result.Message);

        }




        // PUT: api/standings/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TeamStandingCreateUpdateDto dto)
        {
            var standing = new TeamStanding
            {

                Id = id,
                TeamSeasonLeagueId = dto.TeamSeasonLeagueId,
                Played = dto.Played,
                Wins = dto.Wins,
                Losses = dto.Losses,
                PointsFor = dto.PointsFor,
                PointsAgainst = dto.PointsAgainst,
                LeaguePoints = dto.LeaguePoints,
                NoShow = dto.NoShow,
                CurrentStreak = dto.CurrentStreak

            };


            var result = await _teamStandingService.UpdateAsync(standing);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }



            return Ok(result.Message);

        }




        // DELETE: api/standings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var result = await _teamStandingService.DeleteAsync(id);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }



            return Ok(result.Message);

        }



        private static TeamStandingDto MapToDto(TeamStanding standing)
        {
            return new TeamStandingDto
            {
                Id = standing.Id,
                TeamSeasonLeagueId = standing.TeamSeasonLeagueId,
                TeamName = standing.TeamSeasonLeague.Team.Name,
                LeagueName = standing.TeamSeasonLeague.League.Name,
                Played = standing.Played,
                Wins = standing.Wins,
                Losses = standing.Losses,
                PointsFor = standing.PointsFor,
                PointsAgainst = standing.PointsAgainst,
                PointDifference = standing.PointDifference,
                LeaguePoints = standing.LeaguePoints,
                NoShow = standing.NoShow,
                CurrentStreak = standing.CurrentStreak

            };
        }



    }
}
