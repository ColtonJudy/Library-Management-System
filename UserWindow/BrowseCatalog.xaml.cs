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
    /// Interaction logic for BrowseCatalog.xaml
    /// </summary>
    public partial class BrowseCatalog : Window
    {
        MySqlConnection connection;

        public BrowseCatalog()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCatalog();
        }

        private void LoadCatalog()
        {
            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name, IsLoaned FROM Books";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string bookName = reader["Name"].ToString();
                            bool isLoaned = Convert.ToBoolean(reader["IsLoaned"]);

                            string bookListEntry = isLoaned ? bookName + " (not available)" : bookName + " (available)";
                            bookList.Add(bookListEntry);
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

            BookListView.ItemsSource = bookList;
        }
    }
}
