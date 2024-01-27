using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public string selectedBook = "";

        public BookInformation()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeLabels();
        }

        private void InitializeLabels()
        {
            var bookInfo = LibraryBooks.GetBookInfo(selectedBook);

            NameLabel.Content = selectedBook;
            AuthorLabel.Content = "Author: " + bookInfo.Item2 + " " + bookInfo.Item3;
            IDLabel.Content = "Book ID: " + bookInfo.Item1;

            if (bookInfo.Item5)
            {
                LoanedToLabel.Content = "This book is not available. Currently loaned to " + bookInfo.Item4;
            }
            else
            {
                LoanedToLabel.Content = "This book is available";
            }
        }
    }
}
