using Inventrack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvioRutasController : ControllerBase
    {
        private readonly InvenTrackContext _context;

        public EnvioRutasController(InvenTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnvioRuta>>> GetEnvioRutas()
        {
            return await _context.EnvioRutas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnvioRuta>> GetEnvioRuta(int id)
        {
            var envioRuta = await _context.EnvioRutas.FindAsync(id);

            if (envioRuta == null)
            {
                return NotFound();
            }

            return envioRuta;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnvioRuta(int id, EnvioRuta envioRuta)
        {
            if (id != envioRuta.EnvioRutaId)
            {
                return BadRequest();
            }

            _context.Entry(envioRuta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnvioRutaExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<EnvioRuta>> PostEnvioRuta(EnvioRuta envioRuta)
        {
            _context.EnvioRutas.Add(envioRuta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnvioRuta), new { id = envioRuta.EnvioRutaId }, envioRuta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnvioRuta(int id)
        {
            var envioRuta = await _context.EnvioRutas.FindAsync(id);
            if (envioRuta == null)
            {
                return NotFound();
            }

            _context.EnvioRutas.Remove(envioRuta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnvioRutaExists(int id)
        {
            return _context.EnvioRutas.Any(e => e.EnvioRutaId == id);
        }
    }
}
