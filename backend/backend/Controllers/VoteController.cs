using backend.Models.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _voteService;

        public VoteController(VoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteDto voteDto)
        {
            //Console.WriteLine($"Recibido voto: userId={voteDto?.UserId}, pollId={voteDto?.PollId}, opciones={string.Join(",", voteDto?.SelectedOptionIds ?? new List<int>())}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo inválido:");
                foreach (var err in ModelState)
                {
                    Console.WriteLine($"{err.Key}: {string.Join(", ", err.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                return BadRequest(new { message = "Modelo inválido." });
            }

            var result = await _voteService.VoteAsync(voteDto);

            return result == "Voto registrado correctamente."
                ? Ok(new { message = result })
                : BadRequest(new { message = result });
        }


        [HttpGet("hasVoted")]
        public async Task<IActionResult> HasVoted(int pollId, int userId)
        {
            var hasVoted = await _voteService.HasUserVotedAsync(pollId, userId);
            return Ok(hasVoted);
        }

    }
}
