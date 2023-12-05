using EvaluacionGabriel.DTOs;
using EvaluacionGabriel.Models;
using EvaluacionGabriel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace EvaluacionGabriel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {

        private readonly LigaContext _context;
        private readonly ReducirSueldo _reducirSueldo;

        public JugadoresController(LigaContext context, ReducirSueldo reducirSueldo)
        {
         _context = context;
         _reducirSueldo = reducirSueldo;
        }

        #region GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jugadore>>> GetJugadores()
        {

            var dispositivos = await _context.Jugadores.ToListAsync();
            if (dispositivos == null)
            {
                return NotFound("No hay equipos.");

            }
            return Ok(dispositivos);
        }


        [HttpGet("ListaJugadoresDetalles")]
        public async Task<ActionResult<List<DTOAmedida>>> GetListaJugadores()
        {
            var dispositivosDetalles = await _context.Equipos
             .Include(c => c.Jugadores)
            .Select(c => new DTOAmedida
            {
                IdEquipo = c.Id,
                Nombre = c.Nombre,
                Ciudad = c.Ciudad,
                PromedioEdad = c.Jugadores.Average(d => d.Edad),
                ListaJugadores = c.Jugadores.Select(d => new DTOAmedida2
                {
                    IdJugador = d.Id,
                    Nombre = d.Nombre,
                    Edad = d.Edad,
                    Sueldo = d.Sueldo
                }).ToList()

            }).ToListAsync();
            return Ok(dispositivosDetalles);
        }


        [HttpGet("JugadoresLesionados")]

        public async Task<ActionResult<IEnumerable<Jugadore>>> GetLesionados()
        {
            var prueba = _context.Jugadores.GroupBy(d => d.Lesionado)
                .Select(x => new
                {
                    //Descatalogado = (bool)x.Key ? "SI" : "No",
                    Lesionado = x.Key,
                    Cantidad = x.Count(),
                    Jugadores = x.Select(j => new
                      {
                          Nombre = j.Nombre,
                          Equipo = j.Equipo
                      })
                }).ToList();

            return Ok(prueba);
        }


        #endregion

        #region POST

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Jugadore>> PostJugadores(DTOJugadores dTOJugadores)
        {

            var jugadorExiste = await _context.Jugadores.FirstOrDefaultAsync(j => j.Nombre == dTOJugadores.Nombre);
            if (jugadorExiste != null)
            {
                return BadRequest("El jugador ya existe.");
            }

            var hayEquipo = await _context.Jugadores.FirstOrDefaultAsync(x => x.EquipoId == dTOJugadores.EquipoId);

            if (hayEquipo is null)
            {
                return NotFound("El id del equipo no existe");
            }
            var nuevoJugador = new Jugadore()
            {
                Nombre = dTOJugadores.Nombre,
                Edad = dTOJugadores.Edad,
                Sueldo = dTOJugadores.Sueldo,
                Lesionado = dTOJugadores.Lesionado,
                EquipoId = dTOJugadores.EquipoId

            };

            await _context.AddAsync(nuevoJugador);
            await _context.SaveChangesAsync();

            return nuevoJugador;
        }
        #endregion

        #region PUT

        [HttpPut("AumentarSueldo/{idJugador}/{porcentaje}")]
        [Authorize]

        public async Task<ActionResult<Jugadore>> ModificarSueldoJugador(int idJugador, decimal porcentaje)
        {
            var sueldoBase = await _context.Jugadores.FindAsync(idJugador);

            if (sueldoBase == null)
            {
                return NotFound("No se encontró el jugador.");
            }

            decimal aumento = sueldoBase.Sueldo * (porcentaje / 100);
            sueldoBase.Sueldo += aumento;

            _context.Jugadores.Update(sueldoBase);
            await _context.SaveChangesAsync();

            return sueldoBase;
        }

        [HttpPut("reducirSueldoLesionados/{porcentaje}")]
        [Authorize]

        public async Task<ActionResult> ReducirSueldoLesionados(decimal porcentaje)
        {
            var resultado = await _reducirSueldo.ReducirSueldoLesionados(porcentaje);

            if (resultado > 0)
            {
                return Ok("Sueldo reducido a los jugadores lesionados.");
            }
            else
            {
                return BadRequest("No hay jugadores lesionados.");
            }
        }


        #endregion


    }
}
