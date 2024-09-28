using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public GoalsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoals()
        {
            return await _context.Goals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetGoal(Guid id)
        {
            var goal = await _context.Goals.FindAsync(id);
            return goal == null ? NotFound() : goal;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGoal(Guid id, UpdateGoalDto goalDto)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            goal.TargetAmount = goalDto.TargetAmount;
            goal.Progress = goalDto.Progress;
            goal.Deadline = goalDto.Deadline;

            _context.Entry(goal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        public class UpdateGoalDto
        {
            public decimal TargetAmount { get; set; }
            public decimal Progress { get; set; }
            public DateTime Deadline { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<Goal>> PostGoal(CreateGoalDto goalDto)
        {
            var goal = new Goal
            {
                UserId = goalDto.UserId,
                TargetAmount = goalDto.TargetAmount,
                Progress = 0, 
                Deadline = goalDto.Deadline,
                CreatedAt = DateTime.UtcNow
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGoal), new { id = goal.GoalId }, goal);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GoalExists(Guid id)
        {
            return _context.Goals.Any(e => e.GoalId == id);
        }
    }

    public class CreateGoalDto
    {
        public Guid UserId { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime Deadline { get; set; }
    }
}