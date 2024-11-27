using System;
using System.Collections.Generic;

namespace TPCESI_RECETTES.Entities;

public partial class Recette
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public double Tempsprep { get; set; }

    public double Tempscuisson { get; set; }

    public int Difficulte { get; set; }
    


    public virtual ICollection<Categorie> IdCategories { get; set; } = new List<Categorie>();

    public virtual ICollection<Ingredient> Ids { get; set; } = new List<Ingredient>();

    public virtual ICollection<Commentaire> Ids1 { get; set; } = new List<Commentaire>();

    public virtual ICollection<Etape> IdsNavigation { get; set; } = new List<Etape>();
}
