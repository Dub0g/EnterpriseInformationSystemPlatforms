using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Lab_6.Models;

namespace Lab_6.Controllers
{
    public class ServicesController : ODataController
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(_context.Services);

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var service = _context.Services.FirstOrDefault(s => s.ID == key);
            return service == null ? NotFound() : Ok(service);
        }

        public async Task<IActionResult> Post([FromBody] Services service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return Created(service);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Services update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != update.ID) return BadRequest();

            _context.Entry(update).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(s => s.ID == key)) return NotFound();
                throw;
            }
            return Updated(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var service = await _context.Services.FindAsync(key);
            if (service == null) return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}