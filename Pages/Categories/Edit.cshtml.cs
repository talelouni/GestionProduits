using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionProduits.Pages.Categories;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;
    public EditModel(AppDbContext context) => _context = context;

    [BindProperty]
    public Categorie Categorie { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return NotFound();
        Categorie = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Categorie.Produits");
        if (!ModelState.IsValid) return Page();

        var c = await _context.Categories.FindAsync(Categorie.Id);
        if (c == null) return NotFound();

        c.Nom = Categorie.Nom;
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}