using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
using MySql.Data.MySqlClient;

namespace Library_Management_System
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MySqlConnection connection;

        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredEmail = emailTextbox.Text;

            if (IsValidUser(enteredEmail, passwordBox))
            {
                errorLabel.Content = "Login Successful";

                if (IsAdmin(enteredEmail))
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    Close();
                }
                else
                {
                    UserWindow userWindow = new UserWindow();
                    userWindow.SetCurrentUser(enteredEmail);
                    userWindow.Show();
                    Close();
                }
            }
            else
            {
                errorLabel.Content = "Login failed";
            }
        }

        private bool IsValidUser(string email, PasswordBox passwordBox)
        {
            try
            {
                connection.Open();

                string query = "SELECT PasswordHash FROM Users WHERE Email = @Email";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Set the parameters
                    command.Parameters.AddWithValue("@Email", email);
                    string hashedPassword = command.ExecuteScalar() as string ?? "";

                    if(hashedPassword == "")
                    {
                        //invalid user
                        return false;
                    }

                    return BCrypt.Net.BCrypt.EnhancedVerify(passwordBox.Password, hashedPassword);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                errorLabel.Content = "Error: " + exception.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        private bool IsAdmin(string email)
        {
            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND IsAdmin = 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Set the parameters
                    command.Parameters.AddWithValue("@Email", email);

                    // Execute the query and get the result
                    long matchingUserCount = (long)command.ExecuteScalar();

                    connection.Close();

                    // Check if a user with matching credentials was found
                    if (matchingUserCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                errorLabel.Content = "Error: " + exception.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow newAccountWindow = new NewAccountWindow();
            newAccountWindow.Show();
            Close();
        }
    }
}
