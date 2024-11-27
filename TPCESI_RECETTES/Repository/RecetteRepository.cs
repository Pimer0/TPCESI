using Microsoft.EntityFrameworkCore;
using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public class RecetteRepository : GenericRepository<Recette>, IRecetteRepository
{
    public RecetteRepository(DbContext context) : base(context) {}

    // Implémentation des méthodes spécifiques
    public async Task<IEnumerable<Recette>> GetRecettesByCategorie(int categorieId)
    {
        return await _dbSet
            .Where(r => r.IdCategories.Any(c => c.Id == categorieId))
            .ToListAsync();
    }

    public async Task<Recette> GetRecetteWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(r => r.IdCategories)
            .Include(r => r.IdsNavigation)
            .Include(r => r.Ids1)
            .Include(r => r.Ids)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task <Recette> AddRecetteAsync(Recette recette)
    {
       var newRecette = await _dbSet.AddAsync(recette);
         await _context.SaveChangesAsync();
         
            return newRecette.Entity;
    }

    public async Task UpdateRecetteAsync(Recette recette)
    {
        var recetteToUpdate = await _dbSet.FindAsync(recette.Id);
        if (recetteToUpdate != null)
        {
            _context.Entry(recetteToUpdate).CurrentValues.SetValues(recette);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteRecetteAsync(int id)
    {
        var recetteToDelete = await _dbSet.FindAsync(id);
        if (recetteToDelete != null)
        {
            _dbSet.Remove(recetteToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddIngredientToRecetteAsync(int recetteId, int ingredientId)
    {
        var recette = await _dbSet.FindAsync(recetteId);
        var ingredient = await _context.Set<Ingredient>().FindAsync(ingredientId);
        if (recette != null && ingredient != null)
        {
            recette.Ids.Add(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}