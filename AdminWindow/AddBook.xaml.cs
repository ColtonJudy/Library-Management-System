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

        public AddBook()
        {
            InitializeComponent();

        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            string bookTitle = BookNameTextbox.Text;
            string authorFirst = FirstNameTextbox.Text;
            string authorLast = LastNameTextbox.Text;

            if (bookTitle.Length == 0)
            {
                ErrorLabel.Content = "Invalid book title";
            }
            else if (authorFirst.Length == 0)
            {
                ErrorLabel.Content = "Invalid first name";
            }
            else if (authorLast.Length == 0)
            {
                ErrorLabel.Content = "Invalid last name";
            }
            else
            {
                if (LibraryBooks.IsBookNameInUse(bookTitle))
                {
                    ErrorLabel.Content = "Book name already in use";
                }
                else
                {
                    LibraryBooks.AddBook(bookTitle, authorFirst, authorLast);
                    Close();
                }
            }
        }
    }
}
