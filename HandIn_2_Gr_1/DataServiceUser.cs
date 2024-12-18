using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;


namespace HandIn_2_Gr_1
{
    public class DataServiceUser : IDataServiceUser
    {


        // This list is used in multiple functions below.
        public static IList<User>? UserList = new List<User>();


        // This function return a list of all users in database
        public IList<User> GetUsers(int page, int pageSize)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                var calculatedOffSet = (page - 1) * pageSize;

                string query = "SELECT userid, username, useremail FROM Users LIMIT @pagesize OFFSET @offset;";

                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("pagesize", pageSize);
                cmd.Parameters.AddWithValue("offset", calculatedOffSet);

                using var reader = cmd.ExecuteReader();

                UserList = new List<User>();

                while (reader.Read())
                {
                    User user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        userlink = "/api/users/" + reader.GetInt32(0).ToString(),
                        UserName = reader.GetString(1),
                        UserEmail = reader.GetString(2)
                    };

                    UserList.Add(user);
                    
                }

                return UserList;
            }
            catch
            {
            }

            return null;


        }

        //This method does not yet check for existing usernames when making a new user.
        //Should be implemented later
        public void CreateUser(string userName, string password, string useremail)
        {
            var connectionString = Config.GetConnectionString();

            using var connection = new NpgsqlConnection(connectionString);
            if (doesUserNameExitst(userName) == false)
            {
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

                UpdateSeachHistory update = new UpdateSeachHistory();
                update.LogSearchHistory(Config.HardCodedUserID, "User Created, With username: " + userName + ", and password: " + password + " and email: " + useremail);

            }
            else
            {
                Console.WriteLine("something went wrong");
            }

        }



        // This funciton deletes a user from the input-data
        public void DeleteUser(int userID, string password)
        {
            var connectionString = Config.GetConnectionString();

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
            UpdateSeachHistory update = new UpdateSeachHistory();
            update.LogSearchHistory(Config.HardCodedUserID, "User: " + userID.ToString() + " deleted");
        }

        // Currently only returns the last userhit from the database
        public IList<User> SearchUser(string username, string useremail, int userID, int pagesize, int page)
        {

            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                User user = new User();
                connection.Open();
                var calculatedOffSet = (page - 1) * pagesize;

                string query = "SELECT userid, username, useremail FROM users WHERE username ILIKE @username OR useremail ILIKE @useremail OR userID = @userID LIMIT @pagesize OFFSET @offset;";

                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", "%" + username + "%");
                cmd.Parameters.AddWithValue("useremail", "%" + useremail + "%");
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("pagesize", pagesize);
                cmd.Parameters.AddWithValue("offset", calculatedOffSet);
                using var reader = cmd.ExecuteReader();

                var users = new List<User>();

                while (reader.Read())
                {
                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        userlink = "/api/users/" + reader.GetInt32(0).ToString(),
                        UserName = reader.GetString(1),
                        UserEmail = reader.GetString(2)
                    };

                    users.Add(user);
                }

                return users;
            }
            catch
            {
                return null;
            }
        }

        public User SearchUID(int userID)
        {
            //Takes
            //userID = Config.HardCodedUserID();

            var connectionString = Config.GetConnectionString();

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                User user = new User();
                connection.Open();

                string query = "SELECT username, useremail FROM users WHERE userid = @userID;";

                using var cmd = new NpgsqlCommand(query, connection);
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
            var connectionString = Config.GetConnectionString();

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

        //Only used internally
        int findNewUserID()
        {
            var connectionString = Config.GetConnectionString();

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

        bool doesUserNameExitst(string username)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = "SELECT username FROM users WHERE username = @username;";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", username);

                using var reader = cmd.ExecuteReader();

                reader.Read();
                var name = reader.GetString(0);

                if (name.Length >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public void LogSearchHistory(int userid, string searchvalue)
        {
            var connectionString = Config.GetConnectionString();

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "INSERT INTO SearchHistory (userid, searchvalue) VALUES (@userid, @searchvalue);";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("userid", userid);
                cmd.Parameters.AddWithValue("searchvalue", searchvalue);
                cmd.ExecuteNonQuery();
            }
            catch
            {
            }
        }

        public IList<string> ShowSearchHistory(int userid)
        {
            var connectionString = Config.GetConnectionString();
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
            catch
            {
            }
            return searchHistory;
        }
    }
}