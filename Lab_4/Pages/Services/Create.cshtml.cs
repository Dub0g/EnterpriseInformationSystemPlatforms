using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ServicesPages;

public class CreateModel : PageModel
{
    private readonly Lab_4Context _context;

    public CreateModel(Lab_4Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public global::Lab_4.Models.Services Services { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Services.Add(Services);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
