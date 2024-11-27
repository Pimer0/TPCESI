using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public interface IRecetteRepository : IGenericRepository<Recette>
{
    Task<IEnumerable<Recette>> GetRecettesByCategorie(int categorieId);
    Task<Recette> GetRecetteWithDetailsAsync(int id);
    
    Task<Recette> AddRecetteAsync(Recette recette);
    
    Task UpdateRecetteAsync(Recette recette);
    
    Task DeleteRecetteAsync(int id);
    
    Task AddIngredientToRecetteAsync(int recetteId, int ingredientId);
}