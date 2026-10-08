using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_3.Data;
using Lab_3.Models;

namespace Lab_3.Controllers;

public class ServicesController : Controller
{
    private readonly PhotoStudioContext _context;

    public ServicesController(PhotoStudioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services
            .OrderBy(s => s.Name)
            .ToListAsync();

        return View(services);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ID == id);

        if (service == null)
            return NotFound();

        return View(service);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Service service)
    {
        if (!ModelState.IsValid)
            return View(service);

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var service = await _context.Services.FindAsync(id);

        if (service == null)
            return NotFound();

        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Service service)
    {
        if (id != service.ID)
            return NotFound();

        if (!ModelState.IsValid)
            return View(service);

        try
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Services.AnyAsync(s => s.ID == id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ID == id);

        if (service == null)
            return NotFound();

        return View(service);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service != null)
        {
            bool isUsed = await _context.OrderDetails.AnyAsync(od => od.ServiceID == id);

            if (isUsed)
            {
                ModelState.AddModelError(string.Empty,
                    "Неможливо видалити послугу, яка вже використовується в замовленнях.");
                return View(service);
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}