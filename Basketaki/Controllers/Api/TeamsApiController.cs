using Basketaki.Dtos;
using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsApiController : ControllerBase
    {

        private readonly ITeamService _teamService;

        public TeamsApiController(ITeamService teamService)
        {

            _teamService = teamService;

        }



        // GET: api/teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
        {
            var teams = await _teamService.GetAllAsync();

            var result = teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                City = t.City,
                PhotoUrl = t.PhotoUrl,
                CoachId = t.CoachId,
                CoachName = t.Coach != null ? t.Coach.FullName : null

            });


            return Ok(result);

        }



        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetById(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            if (team == null)
            {

                return NotFound();

            }
                

            var result = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City,
                PhotoUrl = team.PhotoUrl,
                CoachId = team.CoachId,
                CoachName = team.Coach != null ? team.Coach.FullName : null
            };



            return Ok(result);

        }



        // POST: api/teams
        [HttpPost]
        public async Task<ActionResult> Create(TeamCreateUpdateDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                City = dto.City,
                PhotoUrl = dto.PhotoUrl,
                CoachId = dto.CoachId
            };


            var result = await _teamService.CreateAsync(team);

            if (!result.Success)
            {

                return BadRequest(result.Message);

            }
                


            return Ok(result.Message);

        }




        // PUT: api/teams/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TeamCreateUpdateDto dto)
        {
            var team = new Team
            {
                Id = id,
                Name = dto.Name,
                City = dto.City,
                PhotoUrl = dto.PhotoUrl,
                CoachId = dto.CoachId
            };

            var result = await _teamService.UpdateAsync(team);

            if (!result.Success)
            {

                return BadRequest(result.Message);
               
            }
               


            return Ok(result.Message);

        }




        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _teamService.DeleteAsync(id);

            if (!result.Success)
            {

                return BadRequest(result.Message);


            }



            return Ok(result.Message);

        }


    }
}
