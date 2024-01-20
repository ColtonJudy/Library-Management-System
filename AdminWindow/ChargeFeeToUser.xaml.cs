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
        MySqlConnection connection;

        public string selectedUser = "";
        public double amountToCharge = 0.0;

        public static readonly Regex previewTextRegex = new Regex(@"^[1-9]\d{0,2}(\.|\.\d{1,2})?$"); //this allows it to end with a decimal while the user is typing
        public static readonly Regex textRegex = new Regex(@"^[1-9]\d{0,2}(\.\d{1,2})?$"); //this does not allow the amount to end with a decimal

        public ChargeFeeToUser()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
            PopulateUserListView();
        }

        public void PopulateUserListView()
        {
            List<string> userList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Email FROM Users";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader["Email"].ToString();
                            userList.Add(user);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
            finally
            {
                connection.Close();
            }

            UserListView.ItemsSource = userList;
        }

        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UserListView.SelectedItem.ToString() ?? "";
        }

        public void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int index = textBox.CaretIndex;
            string newText = textBox.Text.Substring(0, index) + e.Text + textBox.Text.Substring(index); 
            
            //adds the most recently typed character at its index to the original
            e.Handled = !IsPreviewTextAllowed(newText);
        }

        public static bool IsPreviewTextAllowed(string text)
        {
            return previewTextRegex.IsMatch(text);
        }

        public static bool IsTextAllowed(string text)
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
            if (selectedUser != "" && amountToCharge != 0.0)
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Users SET AccountBalance = AccountBalance - @ChargedFee WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserEmail", selectedUser);
                        command.Parameters.AddWithValue("@ChargedFee", amountToCharge);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
                finally
                {
                    connection.Close();
                }

                Close();
            }
        }
    }
}
