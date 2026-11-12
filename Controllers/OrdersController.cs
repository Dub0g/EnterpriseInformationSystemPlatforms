using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Lab_6.Models;

namespace Lab_6.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(_context.Orders);

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == key);
            return order == null ? NotFound() : Ok(order);
        }

        public async Task<IActionResult> Post([FromBody] Orders order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

          
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Created(order);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Orders update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != update.ID) return BadRequest();

            _context.Entry(update).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(o => o.ID == key)) return NotFound();
                throw;
            }
            return Updated(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.ID == key);

            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}