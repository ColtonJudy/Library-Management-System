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
    /// Interaction logic for BookInformation.xaml
    /// </summary>
    public partial class BookInformation : Window
    {
        MySqlConnection connection;

        public string selectedBook;

        public BookInformation()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        public void InitializeLabels()
        {
            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT BookID, AuthorFirst, AuthorLast, LoanedUserEmail, IsLoaned FROM Books WHERE Name = @BookName";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookName", selectedBook);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string bookID = reader["BookID"].ToString() ?? "";
                            string authorFirst = reader["AuthorFirst"].ToString() ?? "";
                            string authorLast = reader["AuthorLast"].ToString() ?? "";
                            string loanedUserEmail = reader["LoanedUserEmail"].ToString() ?? "";
                            bool isLoaned = (bool)reader["IsLoaned"];

                            NameLabel.Content = selectedBook;
                            AuthorLabel.Content = "Author: " + authorFirst + " " + authorLast;
                            IDLabel.Content = "Book ID: " + bookID;

                            if (isLoaned)
                            {
                                LoanedToLabel.Content = "This book is not available. Currently loaned to " + loanedUserEmail;
                            }
                            else
                            {
                                LoanedToLabel.Content = "This book is available";
                            }
                        }
                        else
                        {
                            Console.WriteLine("No matching book found");
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
        }
    }
}
