using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class VenueDAO
    {
        // NOTE: No Console.ReadLine or Console.WriteLine in this class
        // Properties
        private string connectionString;


        //Constructors
        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Methods
        public List<Venue> ListOfAllVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT venue.id VenueId, city.name CityName, state.abbreviation StateAbbreviation, venue.name Name, venue.description Description  FROM VENUE JOIN city ON city.id = venue.city_id JOIN state ON city.state_abbreviation = state.abbreviation ORDER BY name;", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = ConvertReaderToVenue(reader);
                        venues.Add(venue);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);
            }

            return venues;
        }
        private Venue ConvertReaderToVenue(SqlDataReader reader)
        {
            Venue venue = new Venue();
 
            venue.VenueId = Convert.ToInt32(reader["VenueId"]);
            venue.CityName = Convert.ToString(reader["CityName"]);
            venue.StateAbbreviation = Convert.ToString(reader["StateAbbreviation"]);
            venue.Name = Convert.ToString(reader["Name"]);
            venue.Description = Convert.ToString(reader["Description"]);
            return venue;
        }

        public List<string> SearchVenueCatagories(int venueId)
        {
            List<string> categories = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select c.name FROM Venue JOIN category_venue cv ON cv.venue_id = venue.id JOIN category c ON c.id = cv.category_id WHERE venue.id = @venueId", conn);
                    cmd.Parameters.AddWithValue("@venueId", venueId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        categories.Add(Convert.ToString(reader["name"]));
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);
            }

            return categories;
        }
    }
}
