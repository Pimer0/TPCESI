using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public interface ICommentaireRepository : IGenericRepository<Commentaire>
{
    Task<IEnumerable<Commentaire>> GetCommentairesByRecette(int recetteId);
    
    Task<Commentaire> AddCommentaireAsync(Commentaire commentaire);
    
    Task UpdateCommentaireAsync(Commentaire commentaire);
    
    Task DeleteCommentaireAsync(int id);
}