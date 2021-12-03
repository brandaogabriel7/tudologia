using System.Linq;
using System.Threading.Tasks;

using Lime.Messaging.Contents;
using Lime.Protocol;

using NSubstitute;

using Shouldly;

using Take.Api.Tudologia.Facades;
using Take.Api.Tudologia.Facades.Interfaces;
using Take.Api.Tudologia.Models;
using Take.Api.Tudologia.Models.UI;
using Take.Api.Tudologia.Tests.EqualityComparers;
using Take.Api.Tudologia.Tests.TestData;

using Xunit;

namespace Take.Api.Tudologia.Tests
{
    public class ClassesFacadeShould
    {
        private readonly IBlipFacade _blipFacade;
        private readonly ApiSettings _apiSettings;
        private readonly ClassesFacade _classesFacade;

        public ClassesFacadeShould()
        {
            _blipFacade = Substitute.For<IBlipFacade>();
            _apiSettings = new ApiSettings { ClassesResourceName = "classes", AvailabilityMenuText = ClassesFacadeTestData.AVAILABILITY_MENU_TEXT };
            _classesFacade = new ClassesFacade(_blipFacade, _apiSettings);
        }

        [Theory]
        [MemberData(nameof(ClassesFacadeTestData.ReturnALimeMenuContainingClassesAvailabityTestCases), MemberType = typeof(ClassesFacadeTestData))]
        public async Task ReturnALimeMenuContainingClassesAvailabityAsync(JsonDocument classesResources, Select expectedAvailabilityMenu)
        {
            _blipFacade.GetResourceAsync<JsonDocument>(_apiSettings.ClassesResourceName).Returns(Task.FromResult(classesResources));

            var availabilityMenu = await _classesFacade.GetClassesAvailabilityMenuAsync();

            availabilityMenu.ShouldBeOfType<Select>();
            availabilityMenu.Text.ShouldBe(ClassesFacadeTestData.AVAILABILITY_MENU_TEXT);

            availabilityMenu.Options.SequenceEqual(expectedAvailabilityMenu.Options, new SelectOptionsEqualityComparer()).ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(ClassesFacadeTestData.SubscribeAnAttendeeToTheGivenClassTestData), MemberType = typeof(ClassesFacadeTestData))]
        public async Task SubscribeAnAttendeeToTheGivenClassAsync(JsonDocument classesResource, Attendee newAttendee, string chosenClass)
        {
            _blipFacade.GetResourceAsync<JsonDocument>(_apiSettings.ClassesResourceName).Returns(Task.FromResult(classesResource));

            var subscriptionRequest = new SubscriptionRequest
            {
                Attendee = newAttendee,
                ChosenClass = chosenClass
            };

            await _classesFacade.SubscribeAttendeeToClassAsync(subscriptionRequest);

            await _blipFacade.Received().UpdateResourceAsync(_apiSettings.ClassesResourceName, Arg.Is<JsonDocument>(jsonDocument => CheckThatAttendeeWasAddedToAttendeesList(jsonDocument, newAttendee, chosenClass)));
        }
        
        private static bool CheckThatAttendeeWasAddedToAttendeesList(JsonDocument jsonDocument, Attendee newAttendee, string chosenClass)
        {
            var classInformation = jsonDocument[chosenClass] as TudologiaClass;
            return classInformation.Attendees.Contains(newAttendee);
        }
    }
}
