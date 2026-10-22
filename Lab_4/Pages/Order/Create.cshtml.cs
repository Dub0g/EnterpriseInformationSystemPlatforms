using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Pages.Order;

public class CreateModel : PageModel
{
    private readonly Lab_4Context _db;
    public CreateModel(Lab_4Context db) => _db = db;

    [BindProperty]
    public Orders Order { get; set; } = new();

    public async Task OnGetAsync()
    {
        Order.OrderDetails.Add(new OrderDetails { Quantity = 1 });
        await LoadListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Order.ClientID == 0)
            ModelState.AddModelError("Order.ClientID", "Оберіть клієнта");
        if (Order.DeliveryDate < Order.Date)
            ModelState.AddModelError("Order.DeliveryDate", "Дата видачі не може бути раніше дати прийому");
        if (Order.OrderDetails.Count == 0)
            ModelState.AddModelError(string.Empty, "Додайте хоча б одну послугу");

        if (!ModelState.IsValid)
        {
            if (Order.OrderDetails.Count == 0)
                Order.OrderDetails.Add(new OrderDetails { Quantity = 1 });
            await LoadListsAsync();
            return Page();
        }

        _db.Orders.Add(Order);          
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }

    private async Task LoadListsAsync()
    {
        var clients = await _db.Client.AsNoTracking()
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .Select(c => new { c.ID, Name = c.LastName + " " + c.FirstName })
            .ToListAsync();
        ViewData["Clients"] = new SelectList(clients, "ID", "Name", Order.ClientID);
        ViewData["Services"] = new SelectList(await _db.Services.AsNoTracking().OrderBy(s => s.Name).ToListAsync(), "ID", "Name");
    }
}