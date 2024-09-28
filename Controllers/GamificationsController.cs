using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamificationsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public GamificationsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gamification>>> GetGamifications()
        {
            return await _context.Gamifications.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gamification>> GetGamification(Guid id)
        {
            var gamification = await _context.Gamifications.FindAsync(id);
            return gamification == null ? NotFound() : gamification;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGamification(Guid id, UpdateGamificationDto gamificationDto)
        {
            var gamification = await _context.Gamifications.FindAsync(id);
            if (gamification == null)
            {
                return NotFound();
            }

            gamification.Points = gamificationDto.Points;
            gamification.Achievements = gamificationDto.Achievements;

            _context.Entry(gamification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GamificationExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Gamification>> PostGamification(CreateGamificationDto gamificationDto)
        {
            var gamification = new Gamification
            {
                UserId = gamificationDto.UserId,
                Points = gamificationDto.Points,
                Achievements = gamificationDto.Achievements
            };

            _context.Gamifications.Add(gamification);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GamificationExists(gamification.UserId))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtAction(nameof(GetGamification), new { id = gamification.UserId }, gamification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGamification(Guid id)
        {
            var gamification = await _context.Gamifications.FindAsync(id);
            if (gamification == null)
            {
                return NotFound();
            }

            _context.Gamifications.Remove(gamification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GamificationExists(Guid id)
        {
            return _context.Gamifications.Any(e => e.UserId == id);
        }
    }

    public class CreateGamificationDto
    {
        public Guid UserId { get; set; }
        public int Points { get; set; }
        public string Achievements { get; set; }
    }

    public class UpdateGamificationDto
    {
        public int Points { get; set; }
        public string Achievements { get; set; }
    }
}