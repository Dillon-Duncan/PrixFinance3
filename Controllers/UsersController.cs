using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;
using System.Security.Cryptography;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public UsersController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound() : user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserUpdateDto userUpdateDto)
        {
            if (id != userUpdateDto.UserId) return BadRequest();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Email = userUpdateDto.Email ?? user.Email; 
            user.PreferredLanguage = userUpdateDto.PreferredLanguage ?? user.PreferredLanguage; 

            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                user.PasswordHash = HashPassword(userUpdateDto.Password);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserCreateDto userCreateDto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = userCreateDto.Email,
                PasswordHash = HashPassword(userCreateDto.Password),
                CreatedAt = DateTime.UtcNow,
                PreferredLanguage = userCreateDto.PreferredLanguage
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }
    }

    public class UserCreateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PreferredLanguage { get; set; }
    }
    public class UserUpdateDto
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Password { get; set; }
    }
}
