using System;
using System.Collections.Generic;

namespace EvaluacionGabriel.Models;

public partial class Jugadore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public decimal Sueldo { get; set; }

    public bool Lesionado { get; set; }

    public int EquipoId { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;
}
