using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library_Management_System
{
    /// <summary>
    /// Interaction logic for PayFee.xaml
    /// </summary>
    public partial class PayFee : Window
    {
        MySqlConnection connection;

        private string currentUser = "";
        private double currentBalance = 0.0;

        public void SetCurrentUser(string currentUser)
        {
            this.currentUser = currentUser;
        }

        public PayFee()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentBalance();
        }

        private void LoadCurrentBalance()
        {
            if(currentUser != "")
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "SELECT AccountBalance FROM Users WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserEmail", currentUser);

                        // Execute the query and get the result
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currentBalance = Convert.ToDouble(reader["AccountBalance"]);
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

                CurrentBalanceTextBox.Text = "$" + currentBalance.ToString();
            }
        }

        private void PayFeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != "" && currentBalance < 0)
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Users SET AccountBalance = @NewBalance WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserEmail", currentUser);
                        command.Parameters.AddWithValue("@NewBalance", 0.0);
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

                LoadCurrentBalance();
            }
        }
    }
}
