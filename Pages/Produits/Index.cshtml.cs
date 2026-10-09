using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GestionProduits.Pages.Produits;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    public IndexModel(AppDbContext context) => _context = context;

    public int StockTotal { get; set; }
    public decimal PrixMoyen { get; set; }

    public List<Produit> Produits { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Recherche { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Produits.Include(p => p.Categorie).AsQueryable();

        if (!string.IsNullOrWhiteSpace(Recherche))
            query = query.Where(p => p.Nom.Contains(Recherche));

        Produits = await query.OrderBy(p => p.Nom).ToListAsync();

        StockTotal = Produits.Sum(p => p.Stock);
        PrixMoyen = Produits.Any() ? Produits.Average(p => p.Prix) : 0;
    }
}