using Microsoft.EntityFrameworkCore;
using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(DbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Ingredient>> GetIngredientsByRecette(int recetteId)
    {
        return await _dbSet
            .Where(i => i.IdRecettes.Any(r => r.Id == recetteId))
            .ToListAsync();
    }
    
    public async Task<Ingredient> AddIngredientAsync(Ingredient ingredient)
    {
        var newIngredient = await _dbSet.AddAsync(ingredient);
        await _context.SaveChangesAsync();
        
        return newIngredient.Entity;
    }
    
    public async Task UpdateIngredientAsync(Ingredient ingredient)
    {
        var ingredientToUpdate = await _dbSet.FindAsync(ingredient.Id);
        if (ingredientToUpdate != null)
        {
            _context.Entry(ingredientToUpdate).CurrentValues.SetValues(ingredient);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeleteIngredientAsync(int id)
    {
        var ingredientToDelete = await _dbSet.FindAsync(id);
        if (ingredientToDelete != null)
        {
            _dbSet.Remove(ingredientToDelete);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task AddRecetteToIngredientAsync(int ingredientId, int recetteId)
    {
        var ingredient = await _dbSet.FindAsync(ingredientId);
        var recette = await _context.Set<Recette>().FindAsync(recetteId);
        if (ingredient != null && recette != null)
        {
            ingredient.IdRecettes.Add(recette);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task RemoveRecetteFromIngredientAsync(int ingredientId, int recetteId)
    {
        var ingredient = await _dbSet.FindAsync(ingredientId);
        var recette = await _context.Set<Recette>().FindAsync(recetteId);
        if (ingredient != null && recette != null)
        {
            ingredient.IdRecettes.Remove(recette);
            await _context.SaveChangesAsync();
        }
    }
    
    
}