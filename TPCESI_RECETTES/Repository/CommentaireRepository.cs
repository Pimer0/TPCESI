using Microsoft.EntityFrameworkCore;
using TPCESI_RECETTES.Entities;

namespace TPCESI_RECETTES.Repository;

public class CommentaireRepository : GenericRepository<Commentaire>, ICommentaireRepository
{
    public CommentaireRepository(DbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Commentaire>> GetCommentairesByRecette(int recetteId)
    {
       var recette = await _context.Set<Recette>().FindAsync(recetteId);
       return recette.Ids1;
    }

    public async Task<Commentaire> AddCommentaireAsync(Commentaire commentaire)
    {
        var newCommentaire = await _dbSet.AddAsync(commentaire);
        await _context.SaveChangesAsync();
        
        return newCommentaire.Entity;
    }

    public async Task UpdateCommentaireAsync(Commentaire commentaire)
    {
        var commentaireToUpdate = await _dbSet.FindAsync(commentaire.Id);
        if (commentaireToUpdate != null)
        {
            _context.Entry(commentaireToUpdate).CurrentValues.SetValues(commentaire);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCommentaireAsync(int id)
    {
        var commentaireToDelete = await _dbSet.FindAsync(id);
        if (commentaireToDelete != null)
        {
            _dbSet.Remove(commentaireToDelete);
            await _context.SaveChangesAsync();
        }
    }
}