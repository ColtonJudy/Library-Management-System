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
    /// Interaction logic for AdminManageUsers.xaml
    /// </summary>
    public partial class AdminManageUsers : Window
    {
        private string selectedUser = "";

        public AdminManageUsers()
        {
            InitializeComponent();
            PopulateUserListView();
        }

        private void PopulateUserListView()
        {
            UsersListView.ItemsSource = LibraryUsers.GetUsers();
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersListView.SelectedItem.ToString() ?? "";
        }

        private void LoanedBooksButton_Click(object sender, RoutedEventArgs e)
        {
            LoanedBooks loanedBooks = new LoanedBooks();

            if(selectedUser != null && selectedUser != "")
            {
                loanedBooks.selectedUser = selectedUser;
                loanedBooks.ShowDialog();
            }
        }

        private void AccountBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            AccountBalance accountBalance = new AccountBalance();

            if(selectedUser != null && selectedUser != "")
            {
                accountBalance.selectedUser = selectedUser;
                accountBalance.ShowDialog();
            }
        }
    }
}
