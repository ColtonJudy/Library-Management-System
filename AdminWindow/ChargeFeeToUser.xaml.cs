using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ChargeFeeToUser.xaml
    /// </summary>
    public partial class ChargeFeeToUser : Window
    {
        private string selectedUser = "";
        private double amountToCharge = 0.0;

        private static readonly Regex previewTextRegex = new Regex(@"^[1-9]\d{0,2}(\.|\.\d{1,2})?$"); //this allows it to end with a decimal while the user is typing
        private static readonly Regex textRegex = new Regex(@"^[1-9]\d{0,2}(\.\d{1,2})?$"); //this does not allow the amount to end with a decimal

        public ChargeFeeToUser()
        {
            InitializeComponent();
            PopulateUserListView();
        }

        private void PopulateUserListView()
        {
            UserListView.ItemsSource = LibraryUsers.GetUsers();
        }

        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UserListView.SelectedItem.ToString() ?? "";
        }

        private void PreviewInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int index = textBox.CaretIndex;
            string newText = textBox.Text.Substring(0, index) + e.Text + textBox.Text.Substring(index); 
            
            //adds the most recently typed character at its index to the original
            e.Handled = !IsPreviewTextAllowed(newText);
        }

        private static bool IsPreviewTextAllowed(string text)
        {
            return previewTextRegex.IsMatch(text);
        }

        private static bool IsTextAllowed(string text)
        {
            return textRegex.IsMatch(text);
        }

        private void AmountToCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsTextAllowed(AmountToChargeTextBox.Text))
            {
                amountToCharge = Convert.ToDouble(AmountToChargeTextBox.Text);
            }
            else
            {
                amountToCharge = 0.0;
            }
        }

        private void ChargeFeeButton_Click(object sender, RoutedEventArgs e)
        {
            LibraryUsers.ChargeAccount(selectedUser, amountToCharge);
            Close();
        }
    }
}
