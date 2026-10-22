using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly Lab_4Context _db;

    public ClientsController(Lab_4Context db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll() =>
        await _db.Client.AsNoTracking().ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> Get(int id)
    {
        var c = await _db.Client.FindAsync(id);
        return c == null ? NotFound() : c;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Post(Client c)
    {
        _db.Client.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.ID }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Client c)
    {
        if (id != c.ID) return BadRequest();
        if (!await _db.Client.AnyAsync(x => x.ID == id)) return NotFound();
        _db.Entry(c).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Client.FindAsync(id);
        if (c == null) return NotFound();
        _db.Client.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
