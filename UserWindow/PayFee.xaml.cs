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
    /// Interaction logic for PayFee.xaml
    /// </summary>
    public partial class PayFee : Window
    {

        private string currentUser = "";
        private double currentBalance = 0.0;

        public void SetCurrentUser(string currentUser)
        {
            this.currentUser = currentUser;
        }

        public PayFee()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentBalance();
        }

        private void LoadCurrentBalance()
        {
            if(currentUser != "")
            {
                currentBalance = LibraryUsers.LoadBalance(currentUser);

                CurrentBalanceTextBox.Text = "$" + currentBalance.ToString();
            }
        }

        private void PayFeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != "" && currentBalance < 0)
            {
                LibraryUsers.ClearAccountBalance(currentUser);
                LoadCurrentBalance();
            }
        }
    }
}
