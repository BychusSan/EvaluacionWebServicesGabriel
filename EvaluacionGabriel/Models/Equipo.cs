using System;
using System.Collections.Generic;

namespace EvaluacionGabriel.Models;

public partial class Equipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public virtual ICollection<Jugadore> Jugadores { get; set; } = new List<Jugadore>();
}
