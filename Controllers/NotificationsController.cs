using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public NotificationsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return notification == null ? NotFound() : notification;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(Guid id, UpdateNotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.Message = notificationDto.Message;
            notification.IsRead = notificationDto.IsRead;

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        public class UpdateNotificationDto
        {
            public string Message { get; set; }
            public bool IsRead { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification([FromBody] NotificationCreateDto notificationDto)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                IsRead = false, 
                CreatedAt = DateTime.UtcNow 
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationId }, notification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(Guid id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }
    }

    public class NotificationCreateDto
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class NotificationUpdateDto
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}