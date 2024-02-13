using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
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
    /// Interaction logic for NewAccountWindow.xaml
    /// </summary>
    public partial class NewAccountWindow : Window
    {
        private MySqlConnection connection;

        public NewAccountWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        private void LoginExistingButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void CreateNewButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTextbox.Text;
            string lastName = lastNameTextbox.Text;
            string email = emailTextbox.Text;

            //TODO: Make it to where users cannot have a space in their first or last names, or have the same email as somebody who is already in the system

            if (firstName.Length == 0)
            {
                errorLabel.Content = "Invalid first name";
            }
            else if (lastName.Length == 0)
            {
                errorLabel.Content = "Invalid last name";
            }
            else if (email.Length == 0)
            {
                errorLabel.Content = "Invalid email";
            }
            else if (passwordBox.Password.Length < 5)
            {
                errorLabel.Content = "Invalid password length";
            }
            else
            {
                try
                {
                    connection.Open();

                    //check to see if email is already in use
                    string query1 = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (MySqlCommand command = new MySqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        long matchingUserCount = (long)command.ExecuteScalar();

                        if (matchingUserCount > 0)
                        {
                            errorLabel.Content = "Email already in use";
                            return;
                        }
                    }

                    //insert the new user into the table
                    string query2 = "INSERT INTO users (FirstName, LastName, Email, PasswordHash, IsAdmin, AccountBalance) VALUES (@FirstName, @LastName, @Email, @PasswordHash, @IsAdmin, @AccountBalance)";

                    using (MySqlCommand command = new MySqlCommand(query2, connection))
                    {

                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.EnhancedHashPassword(passwordBox.Password, 13));
                        command.Parameters.AddWithValue("@IsAdmin", 0);
                        command.Parameters.AddWithValue("@AccountBalance", 0.00);
                        command.ExecuteNonQuery();

                        UserWindow userWindow = new UserWindow();
                        userWindow.Show();
                        Close();
                    } 
                }
                catch(Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                    errorLabel.Content = "Error: " + exception.Message;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
