using EvaluacionGabriel.Models;

namespace EvaluacionGabriel.DTOs
{
    public class DTOJugadores
    {
            public string Nombre { get; set; } = null!;

            public int Edad { get; set; }

            public decimal Sueldo { get; set; }

            public bool Lesionado { get; set; }

            public int EquipoId { get; set; }

    }
}
