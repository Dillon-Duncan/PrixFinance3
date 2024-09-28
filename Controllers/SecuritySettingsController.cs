using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuritySettingsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public SecuritySettingsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SecuritySetting>>> GetSecuritySettings()
        {
            return await _context.SecuritySettings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SecuritySetting>> GetSecuritySetting(Guid id)
        {
            var securitySetting = await _context.SecuritySettings.FindAsync(id);
            return securitySetting == null ? NotFound() : securitySetting;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSecuritySetting(Guid id, UpdateSecuritySettingDto securitySettingDto)
        {
            var securitySetting = await _context.SecuritySettings.FindAsync(id);
            if (securitySetting == null)
            {
                return NotFound();
            }

            securitySetting.EncryptionKey = securitySettingDto.EncryptionKey;

            _context.Entry(securitySetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecuritySettingExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        public class UpdateSecuritySettingDto
        {
            public string EncryptionKey { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<SecuritySetting>> PostSecuritySetting([FromBody] SecuritySettingCreateDto securitySettingDto)
        {
            var securitySetting = new SecuritySetting
            {
                UserId = Guid.NewGuid(),
                EncryptionKey = securitySettingDto.EncryptionKey,
                LastUpdated = DateTime.UtcNow
            };

            _context.SecuritySettings.Add(securitySetting);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SecuritySettingExists(securitySetting.UserId))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtAction(nameof(GetSecuritySetting), new { id = securitySetting.UserId }, securitySetting);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecuritySetting(Guid id)
        {
            var securitySetting = await _context.SecuritySettings.FindAsync(id);
            if (securitySetting == null) return NotFound();

            _context.SecuritySettings.Remove(securitySetting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SecuritySettingExists(Guid id)
        {
            return _context.SecuritySettings.Any(e => e.UserId == id);
        }
    }

    public class SecuritySettingCreateDto
    {
        public string EncryptionKey { get; set; } = string.Empty;
    }

    public class SecuritySettingUpdateDto
    {
        public Guid UserId { get; set; }
        public string EncryptionKey { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
    }
}