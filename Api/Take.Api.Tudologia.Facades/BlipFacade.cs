using System.Threading.Tasks;

using Lime.Protocol;

using Take.Api.Tudologia.Facades.Interfaces;
using Take.Blip.Client;

using Constants = Take.Api.Tudologia.Models.Constants;
namespace Take.Api.Tudologia.Facades
{
    public class BlipFacade : IBlipFacade
    {
        private readonly ISender _blipClient;

        public BlipFacade(ISender blipClient)
        {
            _blipClient = blipClient;
        }

        /// <inheritdoc/>
        public async Task<T> GetResourceAsync<T>(string resourceKey) where T : Document
        {
            var getResourceCommand = new Command
            {
                Method = CommandMethod.Get,
                Uri = new LimeUri($"{Constants.RESOURCES_URI}/{resourceKey}")
            };

            var responseRequest = await _blipClient.ProcessCommandAsync(getResourceCommand, default);

            if (responseRequest.Status.Equals(CommandStatus.Success))
            {
                return responseRequest.Resource as T;
            }

            return default;
        }


        /// <inheritdoc/>
        public async Task UpdateResourceAsync(string resourceKey, Document resourceContent)
        {
            // TODO: Implementar comando correto para atualizar recurso.
            var setResourceCommand = new Command();

            await _blipClient.SendCommandAsync(setResourceCommand, default);
        }
    }
}
