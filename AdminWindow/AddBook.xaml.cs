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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private MySqlConnection connection;

        public AddBook()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            string bookTitle = bookNameTextbox.Text;
            string authorFirst = firstNameTextbox.Text;
            string authorLast = lastNameTextbox.Text;

            if (bookTitle.Length == 0)
            {
                errorLabel.Content = "Invalid book title";
            }
            else if (authorFirst.Length == 0)
            {
                errorLabel.Content = "Invalid first name";
            }
            else if (authorLast.Length == 0)
            {
                errorLabel.Content = "Invalid last name";
            }
            else
            {
                try
                {
                    connection.Open();

                    //check to see if book already exists
                    //TODO: Make book management independent of title, rely on ID instead.
                    string query1 = "SELECT COUNT(*) FROM Books WHERE Name = @Name";

                    using (MySqlCommand command = new MySqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@Name", bookTitle);

                        long matchingUserCount = (long)command.ExecuteScalar();

                        if (matchingUserCount > 0)
                        {
                            errorLabel.Content = "Email already in use";
                            return;
                        }
                    }

                    //insert the new book into the table
                    string query = "INSERT INTO books (Name, AuthorFirst, AuthorLast, LoanedUserEmail, IsLoaned) VALUES (@Name, @AuthorFirst, @AuthorLast, @LoanedUserEmail, @IsLoaned)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set the parameters
                        command.Parameters.AddWithValue("@Name", bookTitle);
                        command.Parameters.AddWithValue("@AuthorFirst", authorFirst);
                        command.Parameters.AddWithValue("@AuthorLast", authorLast);
                        command.Parameters.AddWithValue("@LoanedUserEmail", null);
                        command.Parameters.AddWithValue("@IsLoaned", false);
                        command.ExecuteNonQuery();

                        Close();
                    }
                }
                catch (Exception exception)
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
