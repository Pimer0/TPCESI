using System;
using System.Collections.Generic;

namespace TPCESI_RECETTES.Entities;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Recette> IdRecettes { get; set; } = new List<Recette>();
}
