using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncDataController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public SyncDataController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SyncData>>> GetSyncData()
        {
            return await _context.SyncData.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SyncData>> GetSyncData(Guid id)
        {
            var syncData = await _context.SyncData.FindAsync(id);
            return syncData == null ? NotFound() : syncData;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSyncData(Guid id, UpdateSyncDataDto syncDataDto)
        {
            var syncData = await _context.SyncData.FindAsync(id);
            if (syncData == null)
            {
                return NotFound();
            }

            syncData.SyncStatus = syncDataDto.SyncStatus;
            syncData.DataChanges = syncDataDto.DataChanges;

            _context.Entry(syncData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SyncDataExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        public class UpdateSyncDataDto
        {
            public string SyncStatus { get; set; }
            public string DataChanges { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<SyncData>> PostSyncData([FromBody] SyncDataCreateDto syncDataDto)
        {
            var syncData = new SyncData
            {
                SyncId = Guid.NewGuid(),
                UserId = syncDataDto.UserId,
                SyncStatus = syncDataDto.SyncStatus,
                LastSyncDate = DateTime.UtcNow,
                DataChanges = syncDataDto.DataChanges
            };

            _context.SyncData.Add(syncData);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSyncData), new { id = syncData.SyncId }, syncData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSyncData(Guid id)
        {
            var syncData = await _context.SyncData.FindAsync(id);
            if (syncData == null) return NotFound();

            _context.SyncData.Remove(syncData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SyncDataExists(Guid id)
        {
            return _context.SyncData.Any(e => e.SyncId == id);
        }
    }

    public class SyncDataCreateDto
    {
        public Guid UserId { get; set; }
        public string SyncStatus { get; set; } = string.Empty;
        public string? DataChanges { get; set; }
    }

    public class SyncDataUpdateDto
    {
        public Guid SyncId { get; set; }
        public Guid UserId { get; set; }
        public string SyncStatus { get; set; } = string.Empty;
        public DateTime? LastSyncDate { get; set; }
        public string? DataChanges { get; set; }
    }
}