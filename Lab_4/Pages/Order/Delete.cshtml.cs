using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;


namespace Lab_4.Pages.OrderPages;

public class DeleteModel : PageModel
{
    private readonly Lab_4Context _context;

    public DeleteModel(Lab_4Context context)
    {
        _context = context;
    }

    [BindProperty]
    public global::Lab_4.Models.Orders Order { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Set<global::Lab_4.Models.Orders>().FirstOrDefaultAsync(m => m.ID == id);
        if (order is null)
        {
            return NotFound();
        }
        else
        {
            Order = order;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _context.Set<global::Lab_4.Models.Orders>().FindAsync(id);
        if (order != null)
        {
            Order = order;
            _context.Set<global::Lab_4.Models.Orders>().Remove(Order);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
