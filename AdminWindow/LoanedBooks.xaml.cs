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
    /// Interaction logic for LoanedBooks.xaml
    /// </summary>
    public partial class LoanedBooks : Window
    {
        public string selectedUser = "";
        private string selectedBook = "";

        public LoanedBooks()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateBookListView();
        }

        private void PopulateBookListView()
        {
            BookListView.ItemsSource = LibraryBooks.GetLoanedBooksByEmail(selectedUser);
        }

        private void BookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBook = BookListView.SelectedItem.ToString() ?? "";
        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook != null && selectedBook != "")
            {
                LibraryBooks.ReturnBook(selectedBook);
                Close();
            }
        }
    }
}
