using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library_Management_System
{
    public static class LibraryBooks
    {
        public static bool IsBookNameInUse(string bookName)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            try
            {
                connection.Open();

                //check to see if book already exists
                //TODO: Make book management independent of title, rely on ID instead.
                string query1 = "SELECT COUNT(*) FROM Books WHERE Name = @Name";

                using (MySqlCommand command = new MySqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@Name", bookName);

                    long matchingUserCount = (long)command.ExecuteScalar();

                    return matchingUserCount > 0;
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

            return true;
        }

        public static void AddBook(string title, string authorFirst, string authorLast) 
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            try
            {
                connection.Open();

                //insert the new book into the table
                string query = "INSERT INTO books (Name, AuthorFirst, AuthorLast, LoanedUserEmail, IsLoaned) VALUES (@Name, @AuthorFirst, @AuthorLast, @LoanedUserEmail, @IsLoaned)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Set the parameters
                    command.Parameters.AddWithValue("@Name", title);
                    command.Parameters.AddWithValue("@AuthorFirst", authorFirst);
                    command.Parameters.AddWithValue("@AuthorLast", authorLast);
                    command.Parameters.AddWithValue("@LoanedUserEmail", null);
                    command.Parameters.AddWithValue("@IsLoaned", false);
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
        }

        public static void DeleteBook(string book)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            if (book != null && book != "")
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Books WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookName", book);
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
            }
        }

        public static void IssueBook(string userEmail, string bookTitle)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            if (userEmail != "" && bookTitle != "")
            {
                try
                {
                    connection.Open();

                    //TODO: Use IDs instead of names to issue books
                    string query = "UPDATE Books SET LoanedUserEmail=@LoanedUserEmail, IsLoaned = @IsLoaned WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LoanedUserEmail", userEmail);
                        command.Parameters.AddWithValue("@IsLoaned", true);
                        command.Parameters.AddWithValue("@BookName", bookTitle);
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
            }
        }

        public static void ReturnBook(string book) 
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            if (book != null && book != "")
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Books SET LoanedUserEmail=@LoanedUserEmail, IsLoaned = @IsLoaned WHERE Name = @BookName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LoanedUserEmail", null);
                        command.Parameters.AddWithValue("@IsLoaned", false);
                        command.Parameters.AddWithValue("@BookName", book);
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
            }
        }

        public static (int, string, string, string, bool) GetBookInfo(string bookTitle)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            int bookID = 0;
            string authorFirst = "", authorLast = "", loanedUserEmail = "";
            bool isLoaned = true;

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT BookID, AuthorFirst, AuthorLast, LoanedUserEmail, IsLoaned FROM Books WHERE Name = @BookName";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookName", bookTitle);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bookID = (int)reader["BookID"];
                            authorFirst = reader["AuthorFirst"].ToString() ?? "";
                            authorLast = reader["AuthorLast"].ToString() ?? "";
                            loanedUserEmail = reader["LoanedUserEmail"].ToString() ?? "";
                            isLoaned = (bool)reader["IsLoaned"];

                        }
                        else
                        {
                            Console.WriteLine("No matching book found");
                        }
                    }
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

            return (bookID, authorFirst, authorLast, loanedUserEmail, isLoaned);
        }

        public static List<string> GetBooks() 
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                string query = "SELECT Name FROM Books";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string book = reader["Name"].ToString() ?? "";
                            bookList.Add(book);
                        }
                    }
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

            return bookList;
        }

        public static List<string> GetBooksWithStatus()
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name, IsLoaned FROM Books";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string bookName = reader["Name"].ToString() ?? "";
                            bool isLoaned = Convert.ToBoolean(reader["IsLoaned"]);

                            string bookListEntry = isLoaned ? bookName + " (not available)" : bookName + " (available)";
                            bookList.Add(bookListEntry);
                        }
                    }
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

            return bookList;
        }

        public static List<string> GetAvailableBooks()
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name FROM Books WHERE IsLoaned = @IsLoaned";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsLoaned", false);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string book = reader["Name"].ToString() ?? "";
                            bookList.Add(book);
                        }
                    }
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

            return bookList;
        }

        public static List<string> GetLoanedBooks()
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name FROM Books WHERE IsLoaned = @IsLoaned";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsLoaned", true);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string book = reader["Name"].ToString() ?? "";
                            bookList.Add(book);
                        }
                    }
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

            return bookList;
        }

        public static List<string> GetLoanedBooksByEmail(string userEmail)
        {
            MySqlConnection connection = new MySqlConnection(LibraryDatabase.GetConnectionString());

            List<string> bookList = new List<string>();

            try
            {
                connection.Open();

                //TODO: Use IDs instead of names to issue books
                string query = "SELECT Name FROM Books WHERE LoanedUserEmail = @LoanedUserEmail";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LoanedUserEmail", userEmail);

                    // Execute the query and get the result
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string book = reader["Name"].ToString() ?? "";
                            bookList.Add(book);
                        }
                    }
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

            return bookList;
        }
    }
}
