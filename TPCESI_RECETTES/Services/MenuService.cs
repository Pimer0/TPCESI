using TPCESI_RECETTES.Entities;
using TPCESI_RECETTES.Repository;

namespace TPCESI_RECETTES.Services;

public class MenuService
{
    private readonly IRecetteRepository _recetteRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly ILogger<MenuService> _logger;
    private readonly ICommentaireRepository _commentaireRepository;

    public MenuService(
        IRecetteRepository recetteRepository, 
        IIngredientRepository ingredientRepository,
        ICommentaireRepository commentaireRepository,
        ILogger<MenuService> logger)
    {
        _recetteRepository = recetteRepository;
        _ingredientRepository = ingredientRepository;
        _commentaireRepository = commentaireRepository;
        _logger = logger;
    }

    public async Task StartMenuAsync()
    {
        while (true)
        {
            AfficherMenuPrincipal();
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    await GererRecettesAsync();
                    break;
                case "2":
                    await RechercherRecettesAsync();
                    break;
                case "3":
                    await GererCommentairesAsync();
                    break;
                case "4":
                    await GererIngredientsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Réessayez.");
                    break;
            }
        }
    }

    private void AfficherMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine("=== GESTION DE RECETTES ===");
        Console.WriteLine("1. Gestion des Recettes");
        Console.WriteLine("2. Rechercher des Recettes");
        Console.WriteLine("3. Gestion des Commentaires");
        Console.WriteLine("4. Gestion des Ingrédients");
        Console.WriteLine("0. Quitter");
        Console.Write("Votre choix : ");
    }

    private async Task GererRecettesAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DES RECETTES ===");
            Console.WriteLine("1. Ajouter une Recette");
            Console.WriteLine("2. Lister les Recettes");
            Console.WriteLine("3. Détails d'une Recette");
            Console.WriteLine("4. Modifier une Recette");
            Console.WriteLine("5. Supprimer une Recette");
            Console.WriteLine("6. Ajouter un Ingrédient à une Recette");
            Console.WriteLine("0. Retour");
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    await AjouterRecetteAsync();
                    break;
                case "2":
                    await ListerRecettesAsync();
                    break;
                case "3":
                    await AfficherDetailsRecetteAsync();
                    break;
                case "4":
                    await ModifierRecetteAsync();
                    break;
                case "5":
                    await SupprimerRecetteAsync();
                    break;
                case "6":
                    await AjouterIngredientARecetteAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Réessayez.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    private async Task AjouterRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UNE RECETTE ===");

        Recette nouvelleRecette = new Recette();

        Console.Write("Nom de la recette : ");
        nouvelleRecette.Nom = Console.ReadLine();

        Console.Write("Temps de préparation (en minutes) : ");
        if (float.TryParse(Console.ReadLine(), out float tempsPrep))
        {
            nouvelleRecette.Tempsprep = tempsPrep;
        }

        Console.Write("Temps de cuisson (en minutes) : ");
        if (float.TryParse(Console.ReadLine(), out float tempsCuisson))
        {
            nouvelleRecette.Tempscuisson = tempsCuisson;
        }

        Console.Write("Difficulté (1-5) : ");
        if (int.TryParse(Console.ReadLine(), out int difficulte))
        {
            nouvelleRecette.Difficulte = difficulte;
        }

        try
        {
            var recetteAjoutee = await _recetteRepository.AddRecetteAsync(nouvelleRecette);
            Console.WriteLine($"Recette ajoutée avec succès ! ID : {recetteAjoutee.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'ajout de la recette");
            Console.WriteLine($"Erreur lors de l'ajout : {ex.Message}");
        }
    }

    private async Task ListerRecettesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== LISTE DES RECETTES ===");

        try
        {
            var recettes = await _recetteRepository.GetAllAsync();

            foreach (var recette in recettes)
            {
                Console.WriteLine($"ID: {recette.Id} - {recette.Nom}");
                Console.WriteLine($"Temps de préparation: {recette.Tempsprep} min");
                Console.WriteLine($"Temps de cuisson: {recette.Tempscuisson} min");
                Console.WriteLine($"Difficulté: {recette.Difficulte}/5");
                Console.WriteLine("---");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la liste des recettes");
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    private async Task AfficherDetailsRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== DÉTAILS D'UNE RECETTE ===");

        Console.Write("Entrez l'ID de la recette : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            try
            {
                var recette = await _recetteRepository.GetRecetteWithDetailsAsync(recetteId);

                if (recette != null)
                {
                    Console.WriteLine($"Nom: {recette.Nom}");
                    Console.WriteLine($"Temps de préparation: {recette.Tempsprep} min");
                    Console.WriteLine($"Temps de cuisson: {recette.Tempscuisson} min");
                    Console.WriteLine($"Difficulté: {recette.Difficulte}/5");

                    // Afficher les catégories
                    Console.WriteLine("Catégories :");
                    foreach (var categorie in recette.IdCategories)
                    {
                        Console.WriteLine($"- {categorie.Nom}");
                    }

                    // Afficher les ingrédients
                    Console.WriteLine("Ingrédients :");
                    foreach (var ingredient in recette.Ids)
                    {
                        Console.WriteLine($"- {ingredient.Nom}");
                    }
                }
                else
                {
                    Console.WriteLine("Recette non trouvée.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de la recette");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task ModifierRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== MODIFIER UNE RECETTE ===");

        Console.Write("Entrez l'ID de la recette à modifier : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            try
            {
                var recetteExistante = await _recetteRepository.GetByIdAsync(recetteId);

                if (recetteExistante != null)
                {
                    Console.Write($"Nom actuel ({recetteExistante.Nom}) : ");
                    string nouveauNom = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nouveauNom))
                        recetteExistante.Nom = nouveauNom;

                    Console.Write($"Temps de préparation actuel ({recetteExistante.Tempsprep}) : ");
                    if (float.TryParse(Console.ReadLine(), out float nouveauTempsPrep))
                        recetteExistante.Tempsprep = nouveauTempsPrep;

                    Console.Write($"Temps de cuisson actuel ({recetteExistante.Tempscuisson}) : ");
                    if (float.TryParse(Console.ReadLine(), out float nouveauTempsCuisson))
                        recetteExistante.Tempscuisson = nouveauTempsCuisson;

                    Console.Write($"Difficulté actuelle ({recetteExistante.Difficulte}) : ");
                    if (int.TryParse(Console.ReadLine(), out int nouvelleDifficulte))
                        recetteExistante.Difficulte = nouvelleDifficulte;

                    await _recetteRepository.UpdateRecetteAsync(recetteExistante);
                    Console.WriteLine("Recette mise à jour avec succès !");
                }
                else
                {
                    Console.WriteLine("Recette non trouvée.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de la recette");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task SupprimerRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== SUPPRIMER UNE RECETTE ===");

        Console.Write("Entrez l'ID de la recette à supprimer : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            try
            {
                await _recetteRepository.DeleteRecetteAsync(recetteId);
                Console.WriteLine("Recette supprimée avec succès !");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la recette");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task AjouterIngredientARecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UN INGRÉDIENT À UNE RECETTE ===");

        Console.Write("ID de la recette : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            Console.Write("ID de l'ingrédient : ");
            if (int.TryParse(Console.ReadLine(), out int ingredientId))
            {
                try
                {
                    await _recetteRepository.AddIngredientToRecetteAsync(recetteId, ingredientId);
                    Console.WriteLine("Ingrédient ajouté à la recette avec succès !");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de l'ajout de l'ingrédient à la recette");
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
        }
    }

    private async Task RechercherRecettesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== RECHERCHER DES RECETTES ===");

        Console.Write("Entrez l'ID de la catégorie : ");
        if (int.TryParse(Console.ReadLine(), out int categorieId))
        {
            try
            {
                var recettes = await _recetteRepository.GetRecettesByCategorie(categorieId);

                Console.WriteLine($"Recettes dans la catégorie {categorieId} :");
                foreach (var recette in recettes)
                {
                    Console.WriteLine($"ID: {recette.Id} - {recette.Nom}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de recettes par catégorie");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
    
    private async Task GererIngredientsAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DES INGRÉDIENTS ===");
            Console.WriteLine("1. Ajouter un Ingrédient");
            Console.WriteLine("2. Lister les Ingrédients d'une Recette");
            Console.WriteLine("3. Modifier un Ingrédient");
            Console.WriteLine("4. Supprimer un Ingrédient");
            Console.WriteLine("5. Ajouter une Recette à un Ingrédient");
            Console.WriteLine("6. Retirer une Recette d'un Ingrédient");
            Console.WriteLine("0. Retour");
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    await AjouterIngredientAsync();
                    break;
                case "2":
                    await ListerIngredientsDeRecetteAsync();
                    break;
                case "3":
                    await ModifierIngredientAsync();
                    break;
                case "4":
                    await SupprimerIngredientAsync();
                    break;
                case "5":
                    await AjouterRecetteAIngredientAsync();
                    break;
                case "6":
                    await RetirerRecetteDeIngredientAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Réessayez.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    private async Task AjouterIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UN INGRÉDIENT ===");

        Ingredient nouvelIngredient = new Ingredient();

        Console.Write("Nom de l'ingrédient : ");
        nouvelIngredient.Nom = Console.ReadLine();

        try
        {
            var ingredientAjoute = await _ingredientRepository.AddIngredientAsync(nouvelIngredient);
            Console.WriteLine($"Ingrédient ajouté avec succès ! ID : {ingredientAjoute.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'ajout de l'ingrédient");
            Console.WriteLine($"Erreur lors de l'ajout : {ex.Message}");
        }
    }

    private async Task ListerIngredientsDeRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== LISTE DES INGRÉDIENTS D'UNE RECETTE ===");

        Console.Write("ID de la recette : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            try
            {
                var ingredients = await _ingredientRepository.GetIngredientsByRecette(recetteId);

                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine($"ID: {ingredient.Id} - {ingredient.Nom}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la liste des ingrédients");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task ModifierIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("=== MODIFIER UN INGRÉDIENT ===");

        Console.Write("Entrez l'ID de l'ingrédient à modifier : ");
        if (int.TryParse(Console.ReadLine(), out int ingredientId))
        {
            try
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(ingredientId);

                if (ingredient != null)
                {
                    Console.Write($"Nom actuel ({ingredient.Nom}) : ");
                    string nouveauNom = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nouveauNom))
                        ingredient.Nom = nouveauNom;

                    await _ingredientRepository.UpdateIngredientAsync(ingredient);
                    Console.WriteLine("Ingrédient mis à jour avec succès !");
                }
                else
                {
                    Console.WriteLine("Ingrédient non trouvé.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de l'ingrédient");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task SupprimerIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("=== SUPPRIMER UN INGRÉDIENT ===");

        Console.Write("Entrez l'ID de l'ingrédient à supprimer : ");
        if (int.TryParse(Console.ReadLine(), out int ingredientId))
        {
            try
            {
                await _ingredientRepository.DeleteIngredientAsync(ingredientId);
                Console.WriteLine("Ingrédient supprimé avec succès !");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'ingrédient");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task AjouterRecetteAIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UNE RECETTE À UN INGRÉDIENT ===");

        Console.Write("ID de l'ingrédient : ");
        if (int.TryParse(Console.ReadLine(), out int ingredientId))
        {
            Console.Write("ID de la recette : ");
            if (int.TryParse(Console.ReadLine(), out int recetteId))
            {
                try
                {
                    await _ingredientRepository.AddRecetteToIngredientAsync(ingredientId, recetteId);
                    Console.WriteLine("Recette ajoutée à l'ingrédient avec succès !");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de l'ajout de la recette à l'ingrédient");
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
        }
    }

    private async Task RetirerRecetteDeIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("=== RETIRER UNE RECETTE D'UN INGRÉDIENT ===");

        Console.Write("ID de l'ingrédient : ");
        if (int.TryParse(Console.ReadLine(), out int ingredientId))
        {
            Console.Write("ID de la recette : ");
            if (int.TryParse(Console.ReadLine(), out int recetteId))
            {
                try
                {
                    await _ingredientRepository.RemoveRecetteFromIngredientAsync(ingredientId, recetteId);
                    Console.WriteLine("Recette retirée de l'ingrédient avec succès !");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du retrait de la recette de l'ingrédient");
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
        }
    }
    private async Task GererCommentairesAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DES COMMENTAIRES ===");
            Console.WriteLine("1. Ajouter un Commentaire");
            Console.WriteLine("2. Lister les Commentaires d'une Recette");
            Console.WriteLine("3. Modifier un Commentaire");
            Console.WriteLine("4. Supprimer un Commentaire");
            Console.WriteLine("0. Retour");
            Console.Write("Votre choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    await AjouterCommentaireAsync();
                    break;
                case "2":
                    await ListerCommentairesParRecetteAsync();
                    break;
                case "3":
                    await ModifierCommentaireAsync();
                    break;
                case "4":
                    await SupprimerCommentaireAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Réessayez.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    private async Task AjouterCommentaireAsync()
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UN COMMENTAIRE ===");

        Commentaire nouveauCommentaire = new Commentaire();

        Console.Write("Texte du commentaire : ");
        nouveauCommentaire.Description = Console.ReadLine();

        Console.Write("ID de la recette : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            nouveauCommentaire.Id = recetteId;
        }

        try
        {
            var commentaireAjoute = await _commentaireRepository.AddCommentaireAsync(nouveauCommentaire);
            Console.WriteLine($"Commentaire ajouté avec succès ! ID : {commentaireAjoute.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'ajout du commentaire");
            Console.WriteLine($"Erreur lors de l'ajout : {ex.Message}");
        }
    }

    private async Task ListerCommentairesParRecetteAsync()
    {
        Console.Clear();
        Console.WriteLine("=== LISTE DES COMMENTAIRES PAR RECETTE ===");

        Console.Write("Entrez l'ID de la recette : ");
        if (int.TryParse(Console.ReadLine(), out int recetteId))
        {
            try
            {
                var commentaires = await _commentaireRepository.GetCommentairesByRecette(recetteId);

                foreach (var commentaire in commentaires)
                {
                    Console.WriteLine($"ID: {commentaire.Id} - {commentaire.Description}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la liste des commentaires");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task ModifierCommentaireAsync()
    {
        Console.Clear();
        Console.WriteLine("=== MODIFIER UN COMMENTAIRE ===");

        Console.Write("Entrez l'ID du commentaire à modifier : ");
        if (int.TryParse(Console.ReadLine(), out int commentaireId))
        {
            try
            {
                var commentaireExistante = await _commentaireRepository.GetByIdAsync(commentaireId);

                if (commentaireExistante != null)
                {
                    Console.Write($"Texte actuel ({commentaireExistante.Description}) : ");
                    string nouveauTexte = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nouveauTexte))
                        commentaireExistante.Description = nouveauTexte;

                    await _commentaireRepository.UpdateCommentaireAsync(commentaireExistante);
                    Console.WriteLine("Commentaire mis à jour avec succès !");
                }
                else
                {
                    Console.WriteLine("Commentaire non trouvé.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification du commentaire");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }

    private async Task SupprimerCommentaireAsync()
    {
        Console.Clear();
        Console.WriteLine("=== SUPPRIMER UN COMMENTAIRE ===");

        Console.Write("Entrez l'ID du commentaire à supprimer : ");
        if (int.TryParse(Console.ReadLine(), out int commentaireId))
        {
            try
            {
                await _commentaireRepository.DeleteCommentaireAsync(commentaireId);
                Console.WriteLine("Commentaire supprimé avec succès !");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du commentaire");
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
}
