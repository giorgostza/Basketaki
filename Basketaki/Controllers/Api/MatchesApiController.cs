using Basketaki.Dtos;
using Basketaki.Services;
using Basketaki.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class MatchesApiController : ControllerBase
    {

        private readonly IMatchService _matchService;

        public MatchesApiController(IMatchService matchService)
        {
            _matchService = matchService;
        }



        // GET: api/matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
        {

            var matches = await _matchService.GetAllAsync();

            var result = matches.Select(MapToDto);

            return Ok(result);

        }



        // GET: api/matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDto>> GetById(int id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
            {

                return NotFound();

            }


            return Ok(MapToDto(match));

        }



        // GET: api/matches/by-league/3
        [HttpGet("by-league/{leagueId}")]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetByLeague(int leagueId)
        {

            var matches = await _matchService.GetAllAsync();

            var result = matches.Where(m => m.LeagueId == leagueId).Select(MapToDto);

            return Ok(result);

        }




        // GET: api/matches/upcoming
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetUpcoming()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var matches = await _matchService.GetAllAsync();

            var result = matches.Where(m => !m.IsPlayed && m.MatchDate >= today).OrderBy(m => m.MatchDate)
                                                                                .ThenBy(m => m.StartTime)
                                                                                .Select(MapToDto);



            return Ok(result);

        }




        // POST: api/matches
        [HttpPost]
        public async Task<ActionResult> Create(MatchCreateUpdateDto dto)
        {
            var match = new Match
            {

                MatchDate = dto.MatchDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CourtId = dto.CourtId,
                LeagueId = dto.LeagueId,
                HomeTeamSeasonLeagueId = dto.HomeTeamSeasonLeagueId,
                AwayTeamSeasonLeagueId = dto.AwayTeamSeasonLeagueId,
                HomeScore = dto.HomeScore,
                AwayScore = dto.AwayScore,
                IsPlayed = dto.IsPlayed

            };



            var result = await _matchService.CreateAsync(match);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }



            return Ok(result.Message);

        }




        // PUT: api/matches/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MatchCreateUpdateDto dto)
        {
            var match = new Match
            {

                Id = id,
                MatchDate = dto.MatchDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CourtId = dto.CourtId,
                LeagueId = dto.LeagueId,
                HomeTeamSeasonLeagueId = dto.HomeTeamSeasonLeagueId,
                AwayTeamSeasonLeagueId = dto.AwayTeamSeasonLeagueId,
                HomeScore = dto.HomeScore,
                AwayScore = dto.AwayScore,
                IsPlayed = dto.IsPlayed

            };


            var result = await _matchService.UpdateAsync(match);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }



            return Ok(result.Message);

        }




        // DELETE: api/matches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var result = await _matchService.DeleteAsync(id);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }


            return Ok(result.Message);

        }



        private static MatchDto MapToDto(Match match)
        {
            return new MatchDto
            {

                Id = match.Id,
                MatchDate = match.MatchDate,
                StartTime = match.StartTime,
                EndTime = match.EndTime,
                CourtId = match.CourtId,
                CourtName = match.Court.Name,
                CourtLocation = match.Court.Location,
                LeagueId = match.LeagueId,
                LeagueName = match.League.Name,
                LeagueSeasonName = match.League.Season.Name,
                HomeTeamSeasonLeagueId = match.HomeTeamSeasonLeagueId,
                HomeTeamName = match.HomeTeamSeasonLeague.Team.Name,
                AwayTeamSeasonLeagueId = match.AwayTeamSeasonLeagueId,
                AwayTeamName = match.AwayTeamSeasonLeague.Team.Name,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                IsPlayed = match.IsPlayed

            };

        }



    }
}
