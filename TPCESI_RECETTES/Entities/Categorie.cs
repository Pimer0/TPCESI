using System;
using System.Collections.Generic;

namespace TPCESI_RECETTES.Entities;

public partial class Categorie
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Recette> Ids { get; set; } = new List<Recette>();
}
