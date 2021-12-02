using System.Threading.Tasks;

using Lime.Protocol;

namespace Take.Api.Tudologia.Facades.Interfaces
{
    public interface IBlipFacade
    {
        /// <summary>
        /// Get a Blip resource, given the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        Task<T> GetResourceAsync<T>(string resourceKey) where T : Document;
    }
}
