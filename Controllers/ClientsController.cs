using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_3.Data;
using Lab_3.Models;

namespace Lab_3.Controllers;

public class ClientsController : Controller
{
    private readonly PhotoStudioContext _context;

    public ClientsController(PhotoStudioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _context.Clients
            .OrderBy(c => c.LastName)
            .ToListAsync();

        return View(clients);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.ID == id);

        if (client == null)
            return NotFound();

        return View(client);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Client client)
    {
        if (!ModelState.IsValid)
            return View(client);

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients.FindAsync(id);

        if (client == null)
            return NotFound();

        return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Client client)
    {
        if (id != client.ID)
            return NotFound();

        if (!ModelState.IsValid)
            return View(client);

        try
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Clients.AnyAsync(c => c.ID == id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.ID == id);

        if (client == null)
            return NotFound();

        return View(client);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client != null)
        {
            bool hasOrders = await _context.Orders.AnyAsync(o => o.ClientID == id);

            if (hasOrders)
            {
                ModelState.AddModelError(string.Empty,
                    "Неможливо видалити клієнта, у якого є замовлення.");
                return View(client);
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}