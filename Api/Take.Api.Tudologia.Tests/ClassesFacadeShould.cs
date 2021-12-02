using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lime.Messaging.Contents;
using Lime.Protocol;

using Newtonsoft.Json.Linq;

using NSubstitute;

using Shouldly;

using Take.Api.Tudologia.Facades;
using Take.Api.Tudologia.Facades.Interfaces;
using Take.Api.Tudologia.Models;
using Take.Api.Tudologia.Models.UI;

using Xunit;

namespace Take.Api.Tudologia.Tests
{
    public class ClassesFacadeShould
    {
        private const string AVAILABILITY_MENU_TEXT = "Escolha uma das turmas:";
        private readonly IBlipFacade _blipFacade;
        private readonly ApiSettings _apiSettings;
        private readonly ClassesFacade _classesFacade;

        public ClassesFacadeShould()
        {
            _blipFacade = Substitute.For<IBlipFacade>();
            _apiSettings = new ApiSettings { ClassesResourceName = "classes", AvailabilityMenuText = AVAILABILITY_MENU_TEXT };
            _classesFacade = new ClassesFacade(_blipFacade, _apiSettings);
        }

        [Theory]
        [MemberData(nameof(ReturnALimeMenuContainingClassesAvailabityTestCases))]
        public async Task ReturnALimeMenuContainingClassesAvailabityAsync(JsonDocument classesResources, Select expectedAvailabilityMenu)
        {
            _blipFacade.GetResourceAsync<JsonDocument>(_apiSettings.ClassesResourceName).Returns(Task.FromResult(classesResources));

            var availabilityMenu = await _classesFacade.GetClassesAvailabilityMenuAsync();

            availabilityMenu.ShouldBe(expectedAvailabilityMenu);
        }

        public static IEnumerable<object[]> ReturnALimeMenuContainingClassesAvailabityTestCases()
        {
            yield return new object[]
            {
                new JsonDocument
                {
                    {
                        "12/21",
                        JObject.FromObject(new TudologiaClass
                        {
                            MaxCapacity = 5,
                            Attendees = new[]
                            {
                                new Attendee
                                {
                                    Name = "Valéria",
                                    Email = "valeria@fakepeople.fake"
                                },
                                new Attendee
                                {
                                    Name = "Guilherme",
                                    Email = "guilherme@fakepeople.fake"
                                }
                            }
                        })
                    }
                },
                new Select
                {
                    Text = AVAILABILITY_MENU_TEXT,
                    Options = new[]
                    {
                        new SelectOption
                        {
                            Text = "Turma 12/21"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new JsonDocument
                {
                    {
                        "12/21",
                        JObject.FromObject(new TudologiaClass
                        {
                            MaxCapacity = 3,
                            Attendees = new[]
                            {
                                new Attendee
                                {
                                    Name = "Valéria",
                                    Email = "valeria@fakepeople.fake"
                                },
                                new Attendee
                                {
                                    Name = "Guilherme",
                                    Email = "guilherme@fakepeople.fake"
                                },
                                new Attendee
                                {
                                    Name = "Renata",
                                    Email = "renata@fakepeople.fake"
                                }
                            }
                        })
                    },
                    {
                        "01/22",
                        JObject.FromObject(new TudologiaClass
                        {
                            MaxCapacity = 3,
                            Attendees = new[]
                            {
                                new Attendee
                                {
                                    Name = "João",
                                    Email = "joao@fakepeople.fake"
                                },
                                new Attendee
                                {
                                    Name = "Jurema",
                                    Email = "jurema@fakepeople.fake"
                                }
                            }
                        })
                    },
                    {
                        "02/22",
                        JObject.FromObject(new TudologiaClass
                        {
                            MaxCapacity = 3,
                            Attendees = Enumerable.Empty<Attendee>()
                        })
                    }
                },
                new Select
                {
                    Text = AVAILABILITY_MENU_TEXT,
                    Options = new[]
                    {
                        new SelectOption
                        {
                            Text = "Turma 01/22"
                        },
                        new SelectOption
                        {
                            Text = "Turma 02/22"
                        }
                    }
                }
            };
        }
    }
}
