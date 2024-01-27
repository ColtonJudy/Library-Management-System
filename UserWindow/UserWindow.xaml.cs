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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private string currentUser = "";

        public void SetCurrentUser(string currentUser)
        {
            this.currentUser = currentUser;
        }

        public UserWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseCatalog browseCatalog = new BrowseCatalog();
            browseCatalog.ShowDialog();
        }

        private void ManageBooksButton_Click(object sender, RoutedEventArgs e)
        {
            ManageCheckedOutBooks manageCheckedOutBooks = new ManageCheckedOutBooks();
            manageCheckedOutBooks.SetCurrentUser(currentUser);
            manageCheckedOutBooks.ShowDialog();
        }

        private void PayFeeButton_Click(object sender, RoutedEventArgs e)
        {
            PayFee payFee = new PayFee();
            payFee.SetCurrentUser(currentUser);
            payFee.ShowDialog();
        }

        private void LibraryHoursButton_Click(object sender, RoutedEventArgs e)
        {
            LibraryHours libraryHours = new LibraryHours();
            libraryHours.ShowDialog();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
