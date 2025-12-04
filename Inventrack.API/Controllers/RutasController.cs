using Inventrack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RutasController : ControllerBase
    {
        private readonly InvenTrackContext _context;

        public RutasController(InvenTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ruta>>> GetRutas()
        {
            return await _context.Rutas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ruta>> GetRuta(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);

            if (ruta == null)
            {
                return NotFound();
            }

            return ruta;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuta(int id, Ruta ruta)
        {
            if (id != ruta.RutaId)
            {
                return BadRequest();
            }

            _context.Entry(ruta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RutaExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Ruta>> PostRuta(Ruta ruta)
        {
            _context.Rutas.Add(ruta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRuta), new { id = ruta.RutaId }, ruta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuta(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }

            _context.Rutas.Remove(ruta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RutaExists(int id)
        {
            return _context.Rutas.Any(e => e.RutaId == id);
        }
    }
}
