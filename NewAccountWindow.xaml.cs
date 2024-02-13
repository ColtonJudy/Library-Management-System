using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

            if (firstName.Length == 0)
            {
                errorLabel.Content = "Invalid first name";
            }
            else if (lastName.Length == 0)
            {
                errorLabel.Content = "Invalid last name";
            }
            else if (email.Length == 0 || !IsValidEmail(email))
            {
                errorLabel.Content = "Invalid email";
            }
            else if (passwordBox.Password.Length == 0)
            {
                errorLabel.Content = "Please enter a password";
            }
            else if (passwordBox.Password.Length < 8)
            {
                errorLabel.Content = "Password must contain at least\n8 characters";
            }
            else if (!passwordBox.Password.Any(char.IsUpper))
            {
                errorLabel.Content = "Password must contain at least\n1 uppercase letter";
            }
            else if (!passwordBox.Password.Any(char.IsLower))
            {
                errorLabel.Content = "Password must contain at least\n1 lowercase letter";
            }
            else if (!passwordBox.Password.Any(char.IsDigit))
            {
                errorLabel.Content = "Password must contain at least\n1 number";
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

        private static bool IsValidEmail(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
    }
}
