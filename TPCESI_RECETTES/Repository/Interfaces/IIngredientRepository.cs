using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public interface IIngredientRepository : IGenericRepository<Ingredient>
{
    Task RemoveRecetteFromIngredientAsync(int ingredientId, int recetteId);
    Task AddRecetteToIngredientAsync(int ingredientId, int recetteId);
    
    Task<IEnumerable<Ingredient>> GetIngredientsByRecette(int recetteId);
    
    Task<Ingredient> AddIngredientAsync(Ingredient ingredient);
    
    Task UpdateIngredientAsync(Ingredient ingredient);
    
    Task DeleteIngredientAsync(int id);
}