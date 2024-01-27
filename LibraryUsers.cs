using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System
{
    public static class LibraryUsers
    {

        //returns the balance of a user
        public static double LoadBalance(string userEmail)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            double balance = 0;

            try
            {
                connection.Open();

                string query = "SELECT AccountBalance FROM Users WHERE Email = @UserEmail";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserEmail", userEmail);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            balance = Convert.ToDouble(reader["AccountBalance"]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
            finally
            {
                connection.Close();
            }

            return balance;
        }

        //subtracts the provided amount from the user's balance
        public static void ChargeAccount(string userEmail, double amountToCharge)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            if (userEmail != "" && amountToCharge != 0.0)
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Users SET AccountBalance = AccountBalance - @ChargedFee WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserEmail", userEmail);
                        command.Parameters.AddWithValue("@ChargedFee", amountToCharge);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //clears a user's balance
        public static void ClearAccountBalance(string userEmail)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            if (userEmail != "")
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Users SET AccountBalance = @ChargedFee WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserEmail", userEmail);
                        command.Parameters.AddWithValue("@ChargedFee", 0);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //returns a list of all users
        public static List<string> GetUsers()
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> userList = new List<string>();

            try
            {
                connection.Open();

                string query = "SELECT Email FROM Users";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader["Email"].ToString() ?? "";
                            userList.Add(user);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
            finally
            {
                connection.Close();
            }

            return userList;
        }
    }
}
