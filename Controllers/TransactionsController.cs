using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrixFinance3.Models;

namespace PrixFinance3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly PrixFinanceDbContext _context;

        public TransactionsController(PrixFinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            return transaction == null ? NotFound() : transaction;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, UpdateTransactionDto transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = transactionDto.Amount;
            transaction.Category = transactionDto.Category;
            transaction.Description = transactionDto.Description;

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        public class UpdateTransactionDto
        {
            public decimal Amount { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = transactionDto.UserId,
                Amount = transactionDto.Amount,
                Date = DateTime.UtcNow,
                Category = transactionDto.Category,
                Description = transactionDto.Description
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(Guid id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }

    public class TransactionCreateDto
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}