using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GestionProduits.Pages.Categories;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;
    public DeleteModel(AppDbContext context) => _context = context;

    public Categorie Categorie { get; set; } = default!;
    public string? Erreur { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return NotFound();
        Categorie = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return RedirectToPage("Index");
        Categorie = c;

        if (await _context.Produits.AnyAsync(p => p.CategorieId == id))
        {
            Erreur = "Impossible de supprimer : cette catégorie contient encore des produits.";
            return Page();
        }

        _context.Categories.Remove(c);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}