using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ServicesPages;

public class DetailsModel : PageModel
{
    private readonly Lab_4Context _context;
    public DetailsModel(Lab_4Context context)
    {
        _context = context;
    }

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
}
