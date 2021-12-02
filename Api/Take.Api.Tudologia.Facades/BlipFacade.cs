using System.Threading.Tasks;

using Lime.Protocol;

using Take.Api.Tudologia.Facades.Interfaces;
using Take.Blip.Client;
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
            // TODO: Implementar comando correto para recuperar o recurso.
            var getResourceCommand = new Command();

            var responseRequest = await _blipClient.ProcessCommandAsync(getResourceCommand, default);

            if (responseRequest.Status.Equals(CommandStatus.Success))
            {
                return responseRequest.Resource as T;
            }

            return default;
        }
    }
}
