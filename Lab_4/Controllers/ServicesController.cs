using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly Lab_4Context _db;

    public ServicesController(Lab_4Context db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Services>>> GetAll() =>
        await _db.Services.AsNoTracking().ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Services>> Get(int id)
    {
        var s = await _db.Services.FindAsync(id);
        return s == null ? NotFound() : s;
    }

    [HttpPost]
    public async Task<ActionResult<Services>> Post(Services s)
    {
        _db.Services.Add(s);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = s.ID }, s);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Services s)
    {
        if (id != s.ID) return BadRequest();
        if (!await _db.Services.AnyAsync(x => x.ID == id)) return NotFound();
        _db.Entry(s).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Services.FindAsync(id);
        if (s == null) return NotFound();
        _db.Services.Remove(s);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
