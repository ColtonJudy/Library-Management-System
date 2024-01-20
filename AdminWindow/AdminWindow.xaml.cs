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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void IssueButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBookToUser issueBookToUser = new IssueBookToUser();
            issueBookToUser.ShowDialog();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnBook returnBook = new ReturnBook();
            returnBook.ShowDialog();
        }

        private void chargeFeeButton_Click(object sender, RoutedEventArgs e)
        {
            ChargeFeeToUser chargeFeeToUser = new ChargeFeeToUser();
            chargeFeeToUser.ShowDialog();
        }

        private void manageBooksButton_Click(object sender, RoutedEventArgs e)
        {
            AdminManageBooks adminManageBooks = new AdminManageBooks();
            adminManageBooks.ShowDialog();
        }

        private void manageUsersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
