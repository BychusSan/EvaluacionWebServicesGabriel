using EvaluacionGabriel.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionGabriel.Services
{
    public class ReducirSueldo
    {

        private readonly LigaContext _context;

        public ReducirSueldo(LigaContext ligaContext)
        {
            _context = ligaContext;
        }

        public async Task<int> ReducirSueldoLesionados(decimal porcentaje)
        {
            var SuelditoBajarLesionados = await _context.Jugadores.Where(j => j.Lesionado).ToListAsync();

            foreach (var jugador in SuelditoBajarLesionados)
            {
                jugador.Sueldo -= jugador.Sueldo * (porcentaje / 100);
                _context.Jugadores.Update(jugador);
            }

            return await _context.SaveChangesAsync();
        }

    }
}
