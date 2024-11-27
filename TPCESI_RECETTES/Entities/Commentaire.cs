using System;
using System.Collections.Generic;

namespace TPCESI_RECETTES.Entities;

public partial class Commentaire
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Recette> IdRecettes { get; set; } = new List<Recette>();
}
