using Microsoft.AspNetCore.Mvc;
using PublicNode.Data;

namespace PublicNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly PublicDbContext _context;

        public PublicController()
        {
            _context = new PublicDbContext();
            _context.Database.EnsureCreated();
        }

        [HttpGet("purchase/{id}")]
        public IActionResult GetPurchase(int id)
        {
            var purchase = _context.Purchases.Find(id);
            if (purchase == null) return NotFound();

            return Ok(new {
                purchase.Id,
                purchase.Encrypted_OID,
                purchase.PurchaseHistory,
                purchase.Amount
            });
        }

        [HttpGet("purchases")]
        public IActionResult GetAllPurchases([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var purchases = _context.Purchases
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            return Ok(purchases);
        }
    }
}