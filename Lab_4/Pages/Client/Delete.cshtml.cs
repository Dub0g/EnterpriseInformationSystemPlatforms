using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ClientPages;

public class DeleteModel : PageModel
{
    private readonly Lab_4Context _context;

    public DeleteModel(Lab_4Context context)
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
        else
        {
            Client = client;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await _context.Client.FindAsync(id);
        if (client != null)
        {
            Client = client;
            _context.Client.Remove(Client);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
