using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Lab_6.Models;


namespace Lab_6.Controllers
{
    public class ClientsController : ODataController
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Clients);
        }

        
        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ID == key);
            if (client == null) return NotFound();
            return Ok(client);
        }

        
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Created(client); 
        }

        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Client update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != update.ID) return BadRequest("ID в URL не співпадає з ID в тілі запиту.");

            _context.Entry(update).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clients.Any(c => c.ID == key)) return NotFound();
                throw;
            }

            return Updated(update); 
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var client = await _context.Clients.FindAsync(key);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}