using System.Threading.Tasks;

using Lime.Messaging.Contents;

using Take.Api.Tudologia.Models;

namespace Take.Api.Tudologia.Facades.Interfaces
{
    public interface IClassesFacade
    {
        /// <summary>
        /// Creates a menu containing the available classes for Tudologia.
        /// </summary>
        Task<Select> GetClassesAvailabilityMenuAsync();
        Task SubscribeAttendeeToClassAsync(SubscriptionRequest subscriptionRequest);
    }
}
