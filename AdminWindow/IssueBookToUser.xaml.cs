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
    /// Interaction logic for IssueBookToUser.xaml
    /// </summary>
    public partial class IssueBookToUser : Window
    {
        MySqlConnection connection;

        public string selectedBook = "";
        public string selectedUser = "";

        public IssueBookToUser()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
            PopulateUserListView();
            PopulateBookListView();
        }

        public void PopulateUserListView()
        {
            List<string> userList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Email FROM Users";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader["Email"].ToString();
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

            UserListView.ItemsSource = userList;
        }

        public void PopulateBookListView()
        {
            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name FROM Books WHERE IsLoaned = @IsLoaned";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsLoaned", false);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string book = reader["Name"].ToString();
                            bookList.Add(book);
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

            BookListView.ItemsSource = bookList;
        }

        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UserListView.SelectedItem.ToString() ?? "";
        }

        private void BookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBook = BookListView.SelectedItem.ToString() ?? "";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != "" && selectedBook != "")
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Books SET LoanedUserEmail=@LoanedUserEmail, IsLoaned = @IsLoaned WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LoanedUserEmail", selectedUser);
                        command.Parameters.AddWithValue("@IsLoaned", true);
                        command.Parameters.AddWithValue("@BookName", selectedBook);
                        command.ExecuteNonQuery();

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

            Close();
        }
    }
}
