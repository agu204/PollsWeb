using backend.Models.Database.Entities;
using backend.Models.Dtos;
using backend.Models.Mappers;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserMapper _userMapper;

        public UserController(UserService userService, UserMapper userMapper)
        {
            _userService = userService;
            _userMapper = userMapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = $"El usuario con el id '{id}' no ha sido encontrado." });
            }

            return Ok(user);
        }

        //obtener todos los usuarios
        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var user = await _userService.GetAllUsersAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpPut("modifyUser")]
        public async Task<IActionResult> ModifyUser([FromBody] UpdateProfileDto userDto)
        {

            // Obtener datos del usuario para modificarse a si mismo
            UserProfileDto userData = await ReadToken();

            if (userData == null)
            {
                Console.WriteLine("Token inválido o usuario no encontrado.");
                return BadRequest("El usuario es null");
            }

            Console.WriteLine($"Usuario autenticado: ID = {userData.UserId}, Email = {userData.Email}");
            userDto.UserId = userData.UserId;
            try
            {
                await _userService.ModifyUserAsync(userDto);
                return Ok("Usuario actualizado correctamente.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("No pudo modificarse el usuario.");
            }
        }


        // Leer datos del token
        private async Task<UserProfileDto> ReadToken()
        {
            try
            {
                string id = User.Claims.FirstOrDefault().Value;
                User user = await _userService.GetUserByIdAsyncNoDto(Int32.Parse(id));
                UserProfileDto userDto = _userMapper.UserProfileToDto(user);
                return userDto;
            }
            catch (Exception)
            {
                Console.WriteLine("La ID del usuario es null");
                return null;
            }
        }
    }
}
