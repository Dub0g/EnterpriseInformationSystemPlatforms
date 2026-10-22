using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly Lab_4Context _db;

    public OrdersController(Lab_4Context db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Orders>>> GetAll() =>
        await _db.Orders.AsNoTracking()
            .Include(o => o.Client)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Service)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Orders>> Get(int id)
    {
        var o = await _db.Orders.AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.OrderDetails).ThenInclude(d => d.Service)
            .FirstOrDefaultAsync(x => x.ID == id);
        return o == null ? NotFound() : o;
    }

    [HttpPost]
    public async Task<ActionResult<Orders>> Post(Orders order)
    {
        if (order.OrderDetails.Count == 0) return BadRequest("Потрібна хоча б одна послуга");
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = order.ID }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Orders input)
    {
        if (id != input.ID) return BadRequest();
        var order = await _db.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.ID == id);
        if (order == null) return NotFound();

        order.ClientID = input.ClientID;
        order.Date = input.Date;
        order.DeliveryDate = input.DeliveryDate;
        _db.OrderDetails.RemoveRange(order.OrderDetails);
        order.OrderDetails = input.OrderDetails.Select(d => new OrderDetails
        {
            ServiceID = d.ServiceID,
            Quantity = d.Quantity,
            UnitPrice = d.UnitPrice
        }).ToList();

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var o = await _db.Orders.FindAsync(id);
        if (o == null) return NotFound();
        _db.Orders.Remove(o);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
