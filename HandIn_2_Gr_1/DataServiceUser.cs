using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public class DataServiceUser : IDataServiceUser
    {

        public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
        public static string filecontent = File.ReadAllText(filepath);

        public static IList<User>? UserList = new List<User>();

        public IList<User> GetUsers()
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Sucess\n");

                using var cmd = new NpgsqlCommand("SELECT username, useremail FROM Users;", connection);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User()
                    {
                        UserName = reader.GetString(0),
                        UserEmail = reader.GetString(1)
                    };

                    UserList.Add(user);

                }
                return UserList;

            }
            catch (Exception ex)
            {
            }

            return null;


        }

        //This method does not yet check for existing userames when making a new user.
        //Should be implemented later
        public void CreateUser(int userID, string userName, string password, string useremail)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "INSERT INTO Users (userid, username, userpassword, useremail) VALUES (@userID, @username, @userpassword, @useremail);";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("username", userName);
                cmd.Parameters.AddWithValue("userpassword", password);
                cmd.Parameters.AddWithValue("useremail", useremail);
                cmd.ExecuteNonQuery();

            }
            catch
            {
                
            }
        }

        public void DeleteUser(int userID, string password)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();

                // Corrected query
                string query = "DELETE FROM Users WHERE userid = '" + userID + "' AND userpassword = '" + password + "';";

                using var cmd = new NpgsqlCommand(query, connection);

                // Execute the command
                int rowsAffected = cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
            }
        }


        // The Following function is coded with help from Co-Pilot
        public IList<User> SearchUser(string username, string useremail, int userid)
        {
            //Define the connection string with PostgreSQL credentials and database name.
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            //Creates list to hold the found users.
            IList<User> foundUsers = new List<User>();

            string searchvalue = !string.IsNullOrEmpty(username) ? username : useremail;

            //Initializing a connections with the database.
            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Database connection succesful! ");

                //SQL query with parameter placeholder| ILIKE used for case insensitive and % for wildcard
                string query = "SELECT username, useremail FROM Users WHERE username ILIKE @SearchTerm or useremail ILIKE @SearchTerm;";

                //Command object for query and connection
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("useremail", useremail);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    {
                        username = reader.GetString(0);
                        useremail = reader.GetString(1);
                    };
                    foundUsers.Add(user);
                }
                LogSearchHistory(userid, searchvalue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return foundUsers;

        }

        // The Following function is coded with help from Co-Pilot
        public void LogSearchHistory(int userid, string searchvalue)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Logging search term...");

                string query = "INSERT INTO SearchHistory (userid, searchvalue) VALUES (@userid, @searchvalue);";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("userid", userid);
                cmd.Parameters.AddWithValue("searchvalue", searchvalue);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Search term logged successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging search history: {ex.Message}");
            }
        }

        // The Following function is coded with help from Co-Pilot
        public IList<string> ShowSearchHistory(int userid)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
            IList<string> searchHistory = new List<string>();
            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Retrieving search history...");
                string query = "SELECT searchvalue FROM SearchHistory WHERE userid = @userid ORDER BY search_date DESC;";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("userid", userid);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string searchValue = reader.GetString(0);
                    searchHistory.Add(searchValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving search history: {ex.Message}");
            }
            return searchHistory;
        }

        // The Following function is coded with help from Co-Pilot
        public bool UserProfile(string username, string userpassword, string useremail)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Database connection successful!");

                //To hold the data from the user
                int UserId = 0;

                //Gets the min and max userID value from the users table
                using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(userid),0) FROM Users;", connection))
                {
                    UserId = (int)cmd.ExecuteScalar();
                }

                int newUserId = UserId + 1;

                if (newUserId < 1 || newUserId > 9999)
                {
                    Console.WriteLine("User ID limit invalid! ");
                    return false;
                }

                string query = "INSERT INTO Users (username, userpassword, useremail) VALUES (@Username, @Password, @Email);";

                using var cmdInsert = new NpgsqlCommand(query, connection);
                cmdInsert.Parameters.AddWithValue("userId", newUserId);
                cmdInsert.Parameters.AddWithValue("Username", username);
                cmdInsert.Parameters.AddWithValue("Password", userpassword);
                cmdInsert.Parameters.AddWithValue("Email", useremail);

                int rowsAffected = cmdInsert.ExecuteNonQuery();

                return rowsAffected > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                return false;
            }
        }

    }
}