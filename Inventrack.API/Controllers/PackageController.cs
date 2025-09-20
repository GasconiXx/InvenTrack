using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : Controller
    {
        private readonly ILogger<PackageController> _logger;

        public PackageController(ILogger<PackageController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetPackageList")]
        public IEnumerable<string> GetPackageList()
        {
            _logger.LogInformation("Fetching package list");
            return new List<string> { "Package1", "Package2", "Package3" };
        }
    }
}
