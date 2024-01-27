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
        private string selectedBook = "";
        private string selectedUser = "";

        public IssueBookToUser()
        {
            InitializeComponent();
            UserListView.ItemsSource = LibraryUsers.GetUsers();
            BookListView.ItemsSource = LibraryBooks.GetAvailableBooks();
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
            LibraryBooks.IssueBook(selectedUser, selectedBook);
            Close();
        }
    }
}
