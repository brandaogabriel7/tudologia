using System;
using System.Threading.Tasks;

using Lime.Messaging.Contents;

using Take.Api.Tudologia.Facades.Interfaces;
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
        public Task<Select> GetClassesAvailabilityMenuAsync()
        {
            throw new NotImplementedException();
        }
    }
}
