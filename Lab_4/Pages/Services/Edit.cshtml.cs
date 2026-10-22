using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ServicesPages;

public class EditModel : PageModel
{
    private readonly Lab_4Context _context;

    public EditModel(Lab_4Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Lab_4.Models.Services Services { get; set; } = default!;

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
        Services = services;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Services).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServicesExists(Services.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ServicesExists(int id)
    {
        return _context.Services.Any(e => e.ID == id);
    }
}
