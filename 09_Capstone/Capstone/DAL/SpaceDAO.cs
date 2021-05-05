using Capstone.DAL;
using Capstone.Models;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SpaceDAO
    {
        private string connectionString;


        //Constructors
        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Methods
        public List<Space> SearchSpacesInOneVenue(int venueId, string startDateString, string endDateString, int numOfAttendees)
        {

            List<string> sqlLongCommands = new List<string>();
            sqlLongCommands.Add("SELECT TOP 5 * FROM space");
            sqlLongCommands.Add("WHERE max_occupancy > @numOfAttendees and venue_id = @venueId");
            sqlLongCommands.Add("and id NOT IN (SELECT Space_id FROM reservation ");
            sqlLongCommands.Add("WHERE ((start_date <= @startDateString) and (end_date >= @endDateString))");
            sqlLongCommands.Add("or ((start_date <= @startDateString) and (end_date >=  @endDateString))");
            sqlLongCommands.Add("or (start_date between @startDateString and  @endDateString  or end_date between @startDateString and  @endDateString))");
            sqlLongCommands.Add("ORDER BY max_occupancy;");

            string sqlLongCommand = String.Join(' ', sqlLongCommands);

            List<Space> availableSpaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlLongCommand, conn);
                    cmd.Parameters.AddWithValue("@venueId", venueId);
                    cmd.Parameters.AddWithValue("@startDateString", startDateString);
                    cmd.Parameters.AddWithValue("@endDateString", endDateString);
                    cmd.Parameters.AddWithValue("@numOfAttendees", numOfAttendees);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = ConvertReaderToSpace(reader);

                        availableSpaces.Add(space);
                    }

                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);

            }
            return availableSpaces;

            /*SELECT * FROM space
            WHERE max_occupancy > 80 and venue_id = 1 and id NOT IN (SELECT Space_id
            FROM reservation WHERE ((start_date <= 'user entered start') 
            and (end_date >= 'user entered start')) or ((start_date <= 'user entered end') and (end_date >= 'user entered end'))
            or (start_date between 'user entered start' and 'user entered end'  or end_date between 'user entered start' and 'user entered end'));
       */
        }


        public List<Space> ListOfAllSpaceInVenue(int venueId)
        {
            List<Space> spaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM space WHERE venue_id = (@venueId);", conn);
                    cmd.Parameters.AddWithValue("@venueId", venueId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = ConvertReaderToSpace(reader);
                        spaces.Add(space);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);
            }

            return spaces;
        }

        public List<Space> ListOfAllAvailableSpaces(string startDateString, string endDateString, int numOfAttendees)
        {

            List<string> sqlLongCommands = new List<string>();
            sqlLongCommands.Add("SELECT * FROM space");
            sqlLongCommands.Add("WHERE max_occupancy > @numOfAttendees");
            sqlLongCommands.Add("and id NOT IN (SELECT Space_id FROM reservation ");
            sqlLongCommands.Add("WHERE ((start_date <= @startDateString) and (end_date >= @endDateString))");
            sqlLongCommands.Add("or ((start_date <= @startDateString) and (end_date >=  @endDateString))");
            sqlLongCommands.Add("or (start_date between @startDateString and  @endDateString  or end_date between @startDateString and  @endDateString))");
            sqlLongCommands.Add("ORDER BY max_occupancy;");

            string sqlLongCommand = String.Join(' ', sqlLongCommands);

            List<Space> availableSpaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlLongCommand, conn);
                    cmd.Parameters.AddWithValue("@startDateString", startDateString);
                    cmd.Parameters.AddWithValue("@endDateString", endDateString);
                    cmd.Parameters.AddWithValue("@numOfAttendees", numOfAttendees);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = ConvertReaderToSpace(reader);

                        availableSpaces.Add(space);
                    }

                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);

            }
            return availableSpaces;
        }


        private Space ConvertReaderToSpace(SqlDataReader reader)
        {
            Space space = new Space();

            space.SpaceId = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.Name = Convert.ToString(reader["name"]);
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            space.HandicapAccessible = Convert.ToBoolean(reader["is_accessible"]);
            if (reader["open_from"] == System.DBNull.Value)
            {
                space.OpenFrom = "";
            }
            else
                space.OpenFrom = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(reader["open_from"]));

            if (reader["open_to"] == System.DBNull.Value)
            {
                space.OpenTo = "";
            }
            else
                space.OpenTo = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(reader["open_to"]));

            return space;
        }

    }
}
