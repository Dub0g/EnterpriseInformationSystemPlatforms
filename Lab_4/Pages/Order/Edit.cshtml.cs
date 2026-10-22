using Lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab_4.Pages.Order;

public class EditModel : PageModel
{
    private readonly Lab_4Context _db;
    public EditModel(Lab_4Context db) => _db = db;

    [BindProperty]
    public Orders Order { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var order = await _db.Orders.AsNoTracking()
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.ID == id);
        if (order == null) return NotFound();

        Order = order;
        await LoadListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
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

        var order = await _db.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.ID == id);
        if (order == null) return NotFound();

        order.ClientID = Order.ClientID;
        order.Date = Order.Date;
        order.DeliveryDate = Order.DeliveryDate;

        // найпростіше: видаляємо старі рядки і додаємо нові з форми
        _db.OrderDetails.RemoveRange(order.OrderDetails);
        order.OrderDetails = Order.OrderDetails.Select(d => new OrderDetails
        {
            ServiceID = d.ServiceID,
            Quantity = d.Quantity,
            UnitPrice = d.UnitPrice
        }).ToList();

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