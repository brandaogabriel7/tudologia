using System.Linq;
using System.Threading.Tasks;

using Lime.Messaging.Contents;
using Lime.Protocol;

using Newtonsoft.Json.Linq;

using Take.Api.Tudologia.Facades.Interfaces;
using Take.Api.Tudologia.Models;
using Take.Api.Tudologia.Models.UI;

namespace Take.Api.Tudologia.Facades
{
    public class ClassesFacade : IClassesFacade
    {
        private readonly IBlipFacade _blipFacade;
        private readonly ApiSettings _apiSettings;

        public ClassesFacade(IBlipFacade blipFacade, ApiSettings apiSettings)
        {
            _blipFacade = blipFacade;
            _apiSettings = apiSettings;
        }

        /// <inheritdoc/>
        public async Task<Select> GetClassesAvailabilityMenuAsync()
        {
            var classesResource = await _blipFacade.GetResourceAsync<JsonDocument>(_apiSettings.ClassesResourceName);

            var availableClasses = classesResource.Where(cr =>
            {
                var classInformation = (cr.Value as JObject).ToObject<TudologiaClass>();
                return classInformation.Attendees.Count() < classInformation.MaxCapacity;
            }).Select(cr => cr.Key);

            var menuOptions = availableClasses.Select(availableClass => new SelectOption { Text = $"Turma {availableClass}" });

            return new Select
            {
                Text = _apiSettings.AvailabilityMenuText,
                Options = menuOptions.ToArray()
            };
        }

        /// <inheritdoc/>
        public async Task SubscribeAttendeeToClassAsync(SubscriptionRequest subscriptionRequest)
        {
            var classesResource = await _blipFacade.GetResourceAsync<JsonDocument>(_apiSettings.ClassesResourceName);

            var classesInformation = classesResource.ToDictionary(cr => cr.Key, cr => (cr.Value as JObject).ToObject<TudologiaClass>());
            
            var attendees = classesInformation[subscriptionRequest.ChosenClass].Attendees.ToList();
            attendees.Add(subscriptionRequest.Attendee);
            
            classesInformation[subscriptionRequest.ChosenClass].Attendees = attendees;

            var updatedClassesResource = new JsonDocument();
            foreach(var classInformation in classesInformation)
            {
                updatedClassesResource.Add(classInformation.Key, classInformation.Value);
            }

            await _blipFacade.UpdateResourceAsync(_apiSettings.ClassesResourceName, updatedClassesResource);
        }
    }
}
