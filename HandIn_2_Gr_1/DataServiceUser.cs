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

        // This function return a list of all users in database
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
        public void CreateUser(string userName, string password, string useremail)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "INSERT INTO Users (userid, username, userpassword, useremail) VALUES (@userID, @username, @userpassword, @useremail);";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("userID", findNewUserID());
                cmd.Parameters.AddWithValue("username", userName);
                cmd.Parameters.AddWithValue("userpassword", password);
                cmd.Parameters.AddWithValue("useremail", useremail);
                cmd.ExecuteNonQuery();

            }
            catch
            {

            }
        }

        // This funciton deletes a user from the input-data
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

        // Currently only returns the last userhit from the database
        public User SearchUser(string username, string useremail, int userID)
        {

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                User user = new User();
                connection.Open();

                string query = "SELECT username, useremail FROM users WHERE username ILIKE @username OR useremail ILIKE @useremail OR userID = @userID LIMIT 1;";

                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", "%" + username + "%");
                cmd.Parameters.AddWithValue("useremail", "%" + useremail + "%");
                cmd.Parameters.AddWithValue("userID", userID);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User()
                    {
                        UserName = reader.GetString(0),
                        UserEmail = reader.GetString(1)
                    };
                }

                // Can be added again if function input is modified to also include logged in user.

                //var searchvalue = username + " " + useremail + " " + userID;
                //LogSearchHistory(userID, searchvalue);

                return user;
            }
            catch
            {
                return null;
            }

        }

        public void UpdateUser(int userID, string userName, string userPassword, string userEmail)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {

                connection.Open();

                if (userName.Length >= 1)
                {
                    string query = "UPDATE Users SET username = @username WHERE userid = @userID;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("userID", userID);
                    cmd.Parameters.AddWithValue("username", userName);
                    cmd.ExecuteNonQuery();

                }
                if (userPassword.Length >= 1)
                {
                    string query = "UPDATE Users SET userpassword = @userpassword WHERE userid = @userID;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("userID", userID);
                    cmd.Parameters.AddWithValue("userpassword", userPassword);
                    cmd.ExecuteNonQuery();

                }
                if (userEmail.Length >= 1)
                {
                    string query = "UPDATE Users SET useremail = @useremail WHERE userid = @userID;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("userID", userID);
                    cmd.Parameters.AddWithValue("useremail", userEmail);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {

            }
        }

        public int findNewUserID()
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "Select MAX(userid) FROM users";
                using var cmd = new NpgsqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();
                int maxID = 0;
                while (reader.Read())
                {
                    maxID = reader.GetInt32(0);
                }
                int NewUserID = maxID + 1;

                return NewUserID;

            }
            catch
            {
                return (0);
            }


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
            catch
            {
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
    }
}