using EvaluacionGabriel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace EvaluacionGabriel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly LigaContext _context;

        public EquiposController(LigaContext context)
        {
            _context = context;
        }


        #region GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipo()
        {

            var dispositivos = await _context.Equipos.ToListAsync();
            if (dispositivos == null)
            {
                return NotFound("No hay equipos.");

            }
            return Ok(dispositivos);
        }

        [HttpGet("PROBAR-EXCEPTION")]
        public async Task<List<Equipo>> GetEquiposException()
        {

            var lista = await _context.Equipos.ToListAsync();
            throw new Exception("Error deliberado");

            return lista;
        }

        #endregion

        #region DELETE

        [HttpDelete("{id:int}")]
        [Authorize]

        public async Task<ActionResult> DeleteEquipo(int id)
        {

            var hayEquipo = await _context.Equipos.FirstOrDefaultAsync(x => x.Id == id);

            if (hayEquipo is null)
            {
                return NotFound("El id del equipo no existe");
            }

            var hayJugadores = await _context.Jugadores.AnyAsync(x => x.EquipoId == id);

            if (hayJugadores)
            {
                return BadRequest("ERROR: hay jugadores en el equipo, no se puede borrar.");
            }

            _context.Remove(hayEquipo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeletearTodo/{id:int}")]
        [Authorize]

        public async Task<ActionResult> DeleteEquipo2(int id)
        {

            var hayEquipo = await _context.Equipos.Include(e => e.Jugadores).FirstOrDefaultAsync(e => e.Id == id);
            if (hayEquipo is null)
            {
                return NotFound("El id del equipo no existe");
            }
            _context.Jugadores.RemoveRange(hayEquipo.Jugadores);
            _context.Remove(hayEquipo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion


    }
}
