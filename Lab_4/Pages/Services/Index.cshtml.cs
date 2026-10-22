using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_4.Models;

namespace Lab_4.Pages.ServicesPages;

public class IndexModel : PageModel
{
    private readonly Lab_4Context _context;

    public IndexModel(Lab_4Context context)
    {
        _context = context;
    }

    public IList<global::Lab_4.Models.Services> Services { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Services = await _context.Services.ToListAsync();
    }
}
