using System.Threading.Tasks;

using Lime.Messaging.Contents;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using Shouldly;

using Take.Api.Tudologia.Controllers;
using Take.Api.Tudologia.Facades.Interfaces;

using Xunit;

namespace Take.Api.Tudologia.Tests
{
    public class ClassesControllerShould
    {
        private readonly IClassesFacade _classesFacade;
        private readonly ClassesController _classesController;

        public ClassesControllerShould()
        {
            _classesFacade = Substitute.For<IClassesFacade>();
            _classesController = new ClassesController(_classesFacade);
        }

        [Fact]
        public async Task ReturnALimeMenuContainingClassesAvailabityAsync()
        {
            var availabilityMenu = new Select
            {
                Text = "Escolha uma turma:",
                Options = new[]
                {
                    new SelectOption
                    {
                        Text = "Turma 12/21"
                    },
                    new SelectOption
                    {
                        Text = "Turma 01/22"
                    },
                    new SelectOption
                    {
                        Text = "Turma 02/22"
                    },
                }
            };

            _classesFacade.GetClassesAvailabilityMenuAsync().Returns(Task.FromResult(availabilityMenu));

            var response = await _classesController.GetClassesAvailabilityMenuAsync();

            response.ShouldBeOfType<OkObjectResult>();

            var okObjectResult = response as OkObjectResult;
            okObjectResult.Value.ShouldBe(availabilityMenu);
        }
    }
}
