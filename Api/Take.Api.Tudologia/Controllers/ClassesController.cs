using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Take.Api.Tudologia.Facades.Interfaces;
using Take.Api.Tudologia.Models;

namespace Take.Api.Tudologia.Controllers
{
    /// <summary>
    /// Handles actions related to Tudologia classes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesFacade _classesFacade;

        /// <summary>
        /// Injects IClassesFacade.
        /// </summary>
        public ClassesController(IClassesFacade classesFacade)
        {
            _classesFacade = classesFacade;
        }

        /// <summary>
        /// Gets a menu containing the classes that are still available.
        /// </summary>
        [HttpGet("availability/menu")]
        public async Task<IActionResult> GetClassesAvailabilityMenuAsync()
        {
            var availabilityMenu = await _classesFacade.GetClassesAvailabilityMenuAsync();
            return Ok(availabilityMenu);
        }

        /// <summary>
        /// Subscribes an attendee to the given class.
        /// </summary>
        [HttpPost("attendees")]
        public async Task<IActionResult> SubscribeAttendeeToClassAsync([FromBody] SubscriptionRequest subscriptionRequest)
        {
            await _classesFacade.SubscribeAttendeeToClassAsync(subscriptionRequest);

            return Ok();
        }
    }
}
