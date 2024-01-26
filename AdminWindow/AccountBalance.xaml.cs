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
    /// Interaction logic for AccountBalance.xaml
    /// </summary>
    public partial class AccountBalance : Window
    {
        MySqlConnection connection;

        public string selectedUser = "";
        public double amountToCharge = 0.0;

        public static readonly Regex previewTextRegex = new Regex(@"^[1-9]\d{0,2}(\.|\.\d{1,2})?$"); //this allows it to end with a decimal while the user is typing
        public static readonly Regex textRegex = new Regex(@"^[1-9]\d{0,2}(\.\d{1,2})?$"); //this does not allow the amount to end with a decimal

        public AccountBalance()
        {
            InitializeComponent();
            connection = new MySqlConnection(LibraryDatabase.GetConnectionString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentBalance();
        }

        public void LoadCurrentBalance()
        {
            string balance = "";

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT AccountBalance FROM Users WHERE Email = @UserEmail";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserEmail", selectedUser);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            balance = reader["AccountBalance"].ToString();
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

            CurrentBalanceTextBox.Text = balance;
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
            if (IsTextAllowed(AmountToChargeTextBox.Text))
            {
                amountToCharge = Convert.ToDouble(AmountToChargeTextBox.Text);
            }
            else
            {
                amountToCharge = 0.0;
            }
        }

        private void DebitAccountButton_Click(object sender, RoutedEventArgs e)
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

                LoadCurrentBalance();
                AmountToChargeTextBox.Text = "";
            }
        }

        private void CreditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != "" && amountToCharge != 0.0)
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Users SET AccountBalance = AccountBalance + @ChargedFee WHERE Email = @UserEmail";

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

                LoadCurrentBalance();
                AmountToChargeTextBox.Text = "";
            }
        }

        private void ClearAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != "")
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Users SET AccountBalance = @ChargedFee WHERE Email = @UserEmail";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserEmail", selectedUser);
                        command.Parameters.AddWithValue("@ChargedFee", 0);
                        command.ExecuteNonQuery();
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

                LoadCurrentBalance();
                AmountToChargeTextBox.Text = "";
            }
        }
    }
}
