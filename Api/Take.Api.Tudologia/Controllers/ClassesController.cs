using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Take.Api.Tudologia.Facades.Interfaces;

namespace Take.Api.Tudologia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesFacade _classesFacade;

        public ClassesController(IClassesFacade classesFacade)
        {
            _classesFacade = classesFacade;
        }

        [HttpGet("availability/menu")]
        public async Task<IActionResult> GetClassesAvailabilityMenuAsync()
        {
            var availabilityMenu = await _classesFacade.GetClassesAvailabilityMenuAsync();
            return Ok(availabilityMenu);
        }
    }
}
