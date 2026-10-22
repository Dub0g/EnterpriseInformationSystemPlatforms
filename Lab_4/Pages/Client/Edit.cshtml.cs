using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ClientPages;

public class EditModel : PageModel
{
    private readonly Lab_4Context _context;

    public EditModel(Lab_4Context context)
    {
        _context = context;
    }

    [BindProperty]
    public global::Lab_4.Models.Client Client { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await _context.Client.FirstOrDefaultAsync(m => m.ID == id);
        if (client is null)
        {
            return NotFound();
        }
        Client = client;
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

        _context.Attach(Client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(Client.ID))
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

    private bool ClientExists(int id)
    {
        return _context.Client.Any(e => e.ID == id);
    }
}
