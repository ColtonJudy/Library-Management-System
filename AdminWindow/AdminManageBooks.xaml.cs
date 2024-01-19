using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    /// Interaction logic for AdminManageBooks.xaml
    /// </summary>
    public partial class AdminManageBooks : Window
    {
        private MySqlConnection connection;
        string selectedBook;

        public AdminManageBooks()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
            PopulateBookListView();
        }

        public void PopulateBookListView()
        {
            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                string query = "SELECT Name FROM Books";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

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

            BooksListView.ItemsSource = bookList;
        }

        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(BooksListView.SelectedItem != null)
            {
                selectedBook = BooksListView.SelectedItem.ToString() ?? "";
            }
        }

        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBook issueBook = new IssueBook();

            if (selectedBook != null && selectedBook != "") 
            {
                issueBook.selectedBook = selectedBook ?? "";
                issueBook.ShowDialog();
            }
        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook != null && selectedBook != "")
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Books SET LoanedUserEmail=@LoanedUserEmail, IsLoaned = @IsLoaned WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LoanedUserEmail", null);
                        command.Parameters.AddWithValue("@IsLoaned", false);
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
        }

        private void BookInformationButton_Click(object sender, RoutedEventArgs e)
        {
            BookInformation bookInformation = new BookInformation();

            if (selectedBook != null && selectedBook != "")
            {
                bookInformation.selectedBook = selectedBook;
                bookInformation.InitializeLabels();
                bookInformation.ShowDialog();
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.ShowDialog();
            PopulateBookListView();
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook != null && selectedBook != "")
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Books WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

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
                PopulateBookListView();
            }
        }
    }
}
