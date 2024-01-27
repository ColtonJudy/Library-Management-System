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
        private string selectedBook = "";

        public AdminManageBooks()
        {
            InitializeComponent();
            PopulateBookListView();
        }

        private void PopulateBookListView()
        {
            BooksListView.ItemsSource = LibraryBooks.GetBooks();
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
            LibraryBooks.ReturnBook(selectedBook);
        }

        private void BookInformationButton_Click(object sender, RoutedEventArgs e)
        {
            BookInformation bookInformation = new BookInformation();

            if (selectedBook != null && selectedBook != "")
            {
                bookInformation.selectedBook = selectedBook;
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
            LibraryBooks.DeleteBook(selectedBook);
            PopulateBookListView();
        }
    }
}
