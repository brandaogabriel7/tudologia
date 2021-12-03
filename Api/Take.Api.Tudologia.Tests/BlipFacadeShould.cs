
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Lime.Messaging.Contents;
using Lime.Protocol;

using NSubstitute;

using Shouldly;

using Take.Api.Tudologia.Facades;
using Take.Blip.Client;

using Xunit;

using Constants = Take.Api.Tudologia.Models.Constants;

namespace Take.Api.Tudologia.Tests
{
    public class BlipFacadeShould
    {
        private const string RESOURCE_KEY = "resourceKey";
        private readonly ISender _blipClient;
        private readonly BlipFacade _blipFacade;

        public BlipFacadeShould()
        {
            _blipClient = Substitute.For<ISender>();
            _blipFacade = new BlipFacade(_blipClient);
        }

        [Theory]
        [InlineData("resourceContent")]
        [InlineData("anotherResourceContent")]
        [InlineData("yetAnotherResourceContent")]
        public async Task GetPlainTextResourceAsync(string resourceContent)
        {
            var command = new Command
            {
                Status = CommandStatus.Success,
                Resource = PlainText.Parse(resourceContent)
            };

            _blipClient.ProcessCommandAsync(Arg.Is(CorrectGetResourceCommand(RESOURCE_KEY)), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(command));

            var response = await _blipFacade.GetResourceAsync<PlainText>(RESOURCE_KEY);
            response.ShouldBeOfType<PlainText>();
            response.Text.ShouldBe(resourceContent);
        }

        [Fact]
        public async Task GetJsonDocumentResourceAsync()
        {
            var resourceContent = new JsonDocument();
            var command = new Command
            {
                Status = CommandStatus.Success,
                Resource = resourceContent
            };

            _blipClient.ProcessCommandAsync(Arg.Is(CorrectGetResourceCommand(RESOURCE_KEY)), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(command));

            var response = await _blipFacade.GetResourceAsync<JsonDocument>(RESOURCE_KEY);
            response.ShouldBeOfType<JsonDocument>();
            response.ShouldBe(resourceContent);
        }

        [Fact]
        public async Task UpdateResourceAsync()
        {
            var resourceContent = new JsonDocument();

            _blipClient.ProcessCommandAsync(Arg.Is(CorrectSetResourceCommand(RESOURCE_KEY, resourceContent)), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(SuccessCommandResponse()));

            await _blipFacade.UpdateResourceAsync(RESOURCE_KEY, resourceContent);
            await _blipClient.Received().SendCommandAsync(Arg.Is(CorrectSetResourceCommand(RESOURCE_KEY, resourceContent)), Arg.Any<CancellationToken>());
        }

        private static Expression<Predicate<Command>> CorrectGetResourceCommand(string resourceKey) =>
                    c => !string.IsNullOrWhiteSpace(c.Id)
                    && c.Method.Equals(CommandMethod.Get)
                    && c.Uri.Path.Equals($"{Constants.RESOURCES_URI}/{resourceKey}");

        private static Expression<Predicate<Command>> CorrectSetResourceCommand<TResource>(string resourceName, TResource resource) =>
                    c => !string.IsNullOrWhiteSpace(c.Id)
                    && c.Method.Equals(CommandMethod.Set)
                    && c.Uri.Path.Equals($"{Constants.RESOURCES_URI}/{resourceName}")
                    && c.Resource.Equals(resource);

        protected static Command SuccessCommandResponse()
        {
            return new Command
            {
                Status = CommandStatus.Success
            };
        }
    }
}
