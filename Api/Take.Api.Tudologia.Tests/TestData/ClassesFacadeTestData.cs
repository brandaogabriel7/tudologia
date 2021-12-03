using System.Collections.Generic;
using System.Linq;

using Lime.Messaging.Contents;
using Lime.Protocol;

using Newtonsoft.Json.Linq;

using Take.Api.Tudologia.Models;

namespace Take.Api.Tudologia.Tests.TestData
{
    public class ClassesFacadeTestData
    {
        public const string AVAILABILITY_MENU_TEXT = "Escolha uma das turmas:";

        public static IEnumerable<object[]> SubscribeAnAttendeeToTheGivenClassTestData()
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
                new Attendee
                {
                    Name = "Chris",
                    Email = "chris@fakepeople.fake"
                },
                "12/21"
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
                new Attendee
                {
                    Name = "Chris",
                    Email = "chris@fakepeople.fake"
                },
                "02/22"
            };
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
