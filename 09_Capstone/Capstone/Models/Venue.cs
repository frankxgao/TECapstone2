using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {

        public int VenueId { get; set; }
        public string CityName { get; set; }
        public string StateAbbreviation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Category { get; set; }


        public string NameAsString ()
        {
            return $"{Name}";
        }
    }
}
