using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public BudgetsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
            return await _context.Budgets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(Guid id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            return budget == null ? NotFound() : budget;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(Guid id, UpdateBudgetDto budgetDto)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            budget.Amount = budgetDto.Amount;
            budget.Category = budgetDto.Category;
            budget.Period = budgetDto.Period;

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        public class UpdateBudgetDto
        {
            public decimal Amount { get; set; }
            public string Category { get; set; }
            public string Period { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(CreateBudgetDto budgetDto)
        {
            var budget = new Budget
            {
                UserId = budgetDto.UserId,
                Category = budgetDto.Category,
                Amount = budgetDto.Amount,
                Period = budgetDto.Period,
                CreatedAt = DateTime.UtcNow 
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBudget), new { id = budget.BudgetId }, budget);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(Guid id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetExists(Guid id)
        {
            return _context.Budgets.Any(e => e.BudgetId == id);
        }
    }

    public class CreateBudgetDto
    {
        public Guid UserId { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public string? Period { get; set; }
    }
}