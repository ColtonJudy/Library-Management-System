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
    /// Interaction logic for ManageCheckedOutBooks.xaml
    /// </summary>
    public partial class ManageCheckedOutBooks : Window
    {
        private string currentUser = "";

        public void SetCurrentUser(string currentUser)
        {
            this.currentUser = currentUser;
        }

        public ManageCheckedOutBooks()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookListView.ItemsSource = LibraryBooks.GetLoanedBooksByEmail(currentUser);
        }
    }
}
