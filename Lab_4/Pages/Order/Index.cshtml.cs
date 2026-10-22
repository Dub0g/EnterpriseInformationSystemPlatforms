using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Pages.Order;

public class IndexModel : PageModel
{
    private readonly Lab_4Context _db;
    public IndexModel(Lab_4Context db) => _db = db;

    public List<Orders> Orders { get; set; } = new();

    public async Task OnGetAsync()
    {
        Orders = await _db.Orders.AsNoTracking()
            .Include(o => o.Client)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Service)
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order != null)
        {
            _db.Orders.Remove(order);   // рядки видаляються каскадно
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}