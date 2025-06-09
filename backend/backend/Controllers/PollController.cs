using backend.Models.Dtos;
using backend.Models.Mappers;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PollController : ControllerBase
{
    private readonly PollService _pollService;
    private readonly PollMapper _pollMapper;

    public PollController(PollService pollService, PollMapper pollMapper)
    {
        _pollService = pollService;
        _pollMapper = pollMapper;
    }

    // Obtener todas las encuestas
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPolls()
    {
        var polls = await _pollService.GetAllPollsAsync();

        if (polls == null || polls.Count == 0)
            return NotFound("No hay encuestas disponibles.");

        var result = polls.Select(_pollMapper.PollToDto);
        return Ok(result);
    }

    // Obtener encuesta por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPollById(int id)
    {
        var poll = await _pollService.GetPollByIdAsync(id);

        if (poll == null)
            return NotFound($"Encuesta con ID {id} no encontrada.");

        return Ok(_pollMapper.PollToDto(poll));
    }

    // Crear encuesta (requiere autenticación)
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePoll([FromBody] PollCreateDto pollCreateDto)
    {
        if (pollCreateDto == null || pollCreateDto.Options.Count < 2)
            return BadRequest("La encuesta debe tener al menos dos opciones.");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var poll = await _pollService.CreatePollAsync(pollCreateDto, userId);
        return Ok(_pollMapper.PollToDto(poll));
    }

    // Finalizar encuesta (solo owner)
    [Authorize]
    [HttpPut("close/{id}")]
    public async Task<IActionResult> ClosePoll(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _pollService.ClosePollAsync(id, userId);
        if (!result)
            return Forbid("No tienes permisos para cerrar esta encuesta.");

        return Ok("Encuesta finalizada.");
    }

    // Eliminar encuesta (solo admin)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePoll(int id)
    {

        var result = await _pollService.DeletePollAsync(id);
        if (!result)
            return NotFound("Encuesta no encontrada.");

        return Ok("Encuesta eliminada.");
    }
}
