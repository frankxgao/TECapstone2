using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int SpaceId { get; set; }
        public string PublicFacingSpaceId
        {
            get
            {
                return (SpaceId + 220).ToString();
            }
        }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public bool HandicapAccessible { get; set; }
        public string OpenFrom { get; set; }
        public string OpenTo { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }

        public override string ToString()
        {
            return $"#{SpaceId}    {Name.PadRight(20)} {OpenFrom.PadRight(10)} {OpenTo.PadRight(10)} ${DailyRate.ToString("0.00")} {MaxOccupancy}";
        }
    }
}
