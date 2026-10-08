using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab_3.Data;
using Lab_3.Models;

namespace Lab_3.Controllers;

public class OrdersController : Controller
{
    private readonly PhotoStudioContext _context;

    public OrdersController(PhotoStudioContext context)
    {
        _context = context;
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.Client)
            .OrderByDescending(o => o.Date)
            .ToListAsync();

        return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Service)
            .FirstOrDefaultAsync(o => o.ID == id);

        if (order == null)
            return NotFound();

        return View(order);
    }

    // GET: Orders/Create
    public IActionResult Create()
    {
        ViewBag.Clients = GetClientsSelectList();
        return View();
    }

    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order)
    {
        if (order.DeliveryDate < order.Date)
        {
            ModelState.AddModelError(nameof(order.DeliveryDate),
                "Дата видачі не може бути раніше дати прийому.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Clients = GetClientsSelectList();
            return View(order);
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Одразу переходимо до деталей, щоб додати позиції замовлення
        return RedirectToAction(nameof(Details), new { id = order.ID });
    }

    // GET: Orders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        ViewBag.Clients = GetClientsSelectList();
        return View(order);
    }

    // POST: Orders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Order order)
    {
        if (id != order.ID)
            return NotFound();

        if (order.DeliveryDate < order.Date)
        {
            ModelState.AddModelError(nameof(order.DeliveryDate),
                "Дата видачі не може бути раніше дати прийому.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Clients = GetClientsSelectList();
            return View(order);
        }

        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Orders.AnyAsync(o => o.ID == id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders
            .Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.ID == id);

        if (order == null)
            return NotFound();

        return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Підтягуємо замовлення РАЗОМ із його деталями
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (order != null)
        {
            // 1. Спочатку видаляємо всі деталі
            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);
            }

            // 2. Потім видаляємо замовлення
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // ----- Керування позиціями замовлення (OrderDetails) -----

    // GET: Orders/CreateDetail?orderId=5
    public async Task<IActionResult> CreateDetail(int? orderId)
    {
        if (orderId == null)
            return NotFound();

        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return NotFound();

        ViewBag.Services = GetServicesSelectList();

        var orderDetail = new OrderDetail { OrderID = orderId.Value, Quantity = 1 };
        return View(orderDetail);
    }

    // POST: Orders/CreateDetail
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDetail(OrderDetail orderDetail)
    {
        ModelState.Remove(nameof(OrderDetail.Order));
        ModelState.Remove(nameof(OrderDetail.Service));

        if (!ModelState.IsValid)
        {
            ViewBag.Services = GetServicesSelectList();
            return View(orderDetail);
        }

        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = orderDetail.OrderID });
    }

    // GET: Orders/EditDetail/3  (3 = OrderDetail.ID)
    public async Task<IActionResult> EditDetail(int? id)
    {
        if (id == null)
            return NotFound();

        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null)
            return NotFound();

        ViewBag.Services = GetServicesSelectList();
        return View(orderDetail);
    }

    // POST: Orders/EditDetail/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDetail(int id, OrderDetail orderDetail)
    {
        if (id != orderDetail.ID)
            return NotFound();

        ModelState.Remove(nameof(OrderDetail.Order));
        ModelState.Remove(nameof(OrderDetail.Service));

        if (!ModelState.IsValid)
        {
            ViewBag.Services = GetServicesSelectList();
            return View(orderDetail);
        }

        try
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.OrderDetails.AnyAsync(od => od.ID == id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Details), new { id = orderDetail.OrderID });
    }

    // GET: Orders/DeleteDetail/3
    public async Task<IActionResult> DeleteDetail(int? id)
    {
        if (id == null)
            return NotFound();

        var orderDetail = await _context.OrderDetails
            .Include(od => od.Service)
            .FirstOrDefaultAsync(od => od.ID == id);

        if (orderDetail == null)
            return NotFound();

        return View(orderDetail);
    }

    // POST: Orders/DeleteDetail/3
    [HttpPost, ActionName("DeleteDetail")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDetailConfirmed(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        int orderId = orderDetail?.OrderID ?? 0;

        if (orderDetail != null)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    // ----- Допоміжні методи -----

    private SelectList GetClientsSelectList()
    {
        var clients = _context.Clients
            .OrderBy(c => c.LastName)
            .Select(c => new { c.ID, FullName = c.LastName + " " + c.FirstName })
            .ToList();

        return new SelectList(clients, "ID", "FullName");
    }

    private SelectList GetServicesSelectList()
    {
        return new SelectList(
            _context.Services.OrderBy(s => s.Name).ToList(),
            "ID", "Name");
    }
}