using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {

        public int ReservationNumber { get; set; }
        public int PublicFacingReservationNumber
        {
            get
            {
                return (ReservationNumber + 86400);
            }
        }
        public int SpaceId { get; set; }
        public string SpaceName { get; set; }
        public string VenueName { get; set; }
        public int NumberOfAttendees { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReservedFor { get; set; }
        public decimal TotalCost { get; private set; }

        public void GenerateTotalCost(decimal spaceCost)
        {
            TotalCost = spaceCost * (decimal)(EndDate - StartDate).TotalDays;
            return;
        }

        public override string ToString()
        {
            return $"{VenueName.PadRight(35)}{SpaceName.PadRight(35)}{ReservedFor.PadRight(30)}{StartDate.ToString().Split()[0].PadRight(13)}{EndDate.ToString().Split()[0]}";
        }
    }
}
