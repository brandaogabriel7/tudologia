using System.Collections.Generic;

namespace Take.Api.Tudologia.Models
{
    public class TudologiaClass
    {
        public int MaxCapacity { get; set; }
        public IEnumerable<Attendee> Attendees { get; set; }
    }
}
