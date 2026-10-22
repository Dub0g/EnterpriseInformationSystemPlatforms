using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ServicesPages;

public class DeleteModel : PageModel
{
    private readonly Lab_4Context _context;

    public DeleteModel(Lab_4Context context)
    {
        _context = context;
    }

    [BindProperty]
    public global::Lab_4.Models.Services Services { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var services = await _context.Services.FirstOrDefaultAsync(m => m.ID == id);
        if (services is null)
        {
            return NotFound();
        }
        else
        {
            Services = services;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var services = await _context.Services.FindAsync(id);
        if (services != null)
        {
            Services = services;
            _context.Services.Remove(Services);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
