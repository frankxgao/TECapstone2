using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class ReservationDAO
    {
        // NOTE: No Console.ReadLine or Console.WriteLine in this class
        // Properties
        private string connectionString;


        //Constructors
        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Methods
        public List<Reservation> ListOfUpcomingReservation(string currentDate, string outBy30Days)
        {
            List<string> sqlLongCommands = new List<string>();
            sqlLongCommands.Add("Select v.name venuename, s.name spacename, reserved_for, start_date, end_date");
            sqlLongCommands.Add("FROM reservation JOIN space s On s.id = reservation.space_id JOIN venue v on v.id = s.venue_id ");
            sqlLongCommands.Add("WHERE (start_date > @currentDate) and (start_date < @outBy30Days)");
            sqlLongCommands.Add("ORDER BY start_date;");

            string sqlLongCommand = String.Join(' ', sqlLongCommands);


            List<Reservation> reservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlLongCommand, conn);
                    cmd.Parameters.AddWithValue("@currentDate", currentDate);
                    cmd.Parameters.AddWithValue("@outBy30Days", outBy30Days);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = ConvertReaderReservation(reader);
                        reservations.Add(reservation);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);
            }

            return reservations;
        }

        public int AddReservation(int spaceId, string startDateString, string endDateString, int numOfAttendees, string reservedFor)
        {
            int newReservationId = new int();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES ((@spaceId), (@numOfAttendees), (@startDateString), (@endDateString),  (@reservedFor))", conn);
                    cmd.Parameters.AddWithValue("@spaceId", spaceId);
                    cmd.Parameters.AddWithValue("@startDateString", startDateString);
                    cmd.Parameters.AddWithValue("@endDateString", endDateString);
                    cmd.Parameters.AddWithValue("@numOfAttendees", numOfAttendees);
                    cmd.Parameters.AddWithValue("@reservedFor", reservedFor);
                    SqlDataReader reader = cmd.ExecuteReader();

               
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation;", conn);
                    newReservationId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There what an error connecting to the database");
                Console.WriteLine(ex.Message);
            }
            return newReservationId;
        }

        private Reservation ConvertReaderReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();
           // reservation.ReservationNumber = Convert.ToInt32(reader["reservation_id"]);
           // reservation.SpaceId = Convert.ToInt32(reader["space_id"]);
           // reservation.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
            reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
            reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);
            reservation.VenueName = Convert.ToString(reader["venuename"]);
            reservation.SpaceName = Convert.ToString(reader["spacename"]);
            return reservation;
        }

        


       
    }
}
