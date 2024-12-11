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


namespace HandIn_2_Gr_1
{
    public class DataServiceUser : IDataServiceUser
    {

        public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
        public static string filecontent = File.ReadAllText(filepath);

        public static IList<User>? UserList = new List<User>();

        // This function return a list of all users in database
        public IList<User> GetUsers(int page, int pageSize)
        {
            // If people use too big or small a pagesize, we will revert it to 50
            if (pageSize > 50 || pageSize <= 0)
            {
                pageSize = 50;
            }

            // To make sure no-one is searching for page -1
            if (page <= 0)
            {
                page = 1;
            }

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
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

        /*
        public Title updateUser(string nconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

            using var connection = new NpgsqlConnection(connectionString);
            if (doesTconstExist(tconst))
            {
                try
                {
                    connection.Open();

                    if (titletype.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET titletype = @titletype WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("titletype", titletype);
                        cmd.ExecuteNonQuery();
                    }


                    if (primaryTitle.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET primaryTitle = @primaryTitle WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("primaryTitle", primaryTitle);
                        cmd.ExecuteNonQuery();
                    }


                    if (originalTitle.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET originalTitle = @originalTitle WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("originalTitle", originalTitle);
                        cmd.ExecuteNonQuery();
                    }


                    if (!string.IsNullOrEmpty(isAdult))
                    {
                        bool isAdultValue = isAdult.ToLower() switch
                        {
                            "t" => true,
                            "f" => false,
                            _ => false
                        };

                        string query = "UPDATE title_basics SET isAdult = @isAdult WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("isAdult", isAdultValue);
                        cmd.ExecuteNonQuery();
                    }


                    if (startyear.Length == 4)
                    {
                        string query = "UPDATE title_basics SET startyear = @startyear WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("startyear", startyear);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Startyear");
                    }


                    if (endyear.Length == 4)
                    {
                        string query = "UPDATE title_basics SET endyear = @endyear WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("endyear", endyear);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Endyear");
                    }

                    //A limitation of this way, is that titles less that 1 minute will be need to have the value 1 minute
                    if (runtimeMinutes > 0)
                    {
                        string query = "UPDATE title_basics SET runtimeMinutes = @runtimeMinutes WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("runtimeMinutes", runtimeMinutes);
                        cmd.ExecuteNonQuery();
                    }


                    if (genres.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET genres = @genres WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("genres", genres);
                        cmd.ExecuteNonQuery();
                    }


                    if (posterlink.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET poster = @poster WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("poster", posterlink);
                        cmd.ExecuteNonQuery();
                    }

                    if (plot.Length >= 1)
                    {
                        string query = "UPDATE title_basics SET plot = @plot WHERE tconst = @tconst;";
                        using var cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("tconst", tconst);
                        cmd.Parameters.AddWithValue("plot", plot);
                        cmd.ExecuteNonQuery();
                    }

                    return new Title
                    {
                        Tconst = tconst,
                        TitleType = titletype,
                        PrimaryTitle = primaryTitle,
                        OriginalTitle = originalTitle,
                        IsAdult = isAdult,
                        StartYear = startyear,
                        EndYear = endyear,
                        RuntimeMinutes = runtimeMinutes,
                        //Genre = genres,
                        PosterLink = posterlink,
                        plot = plot
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
            return null;
        }
        */

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

        public User SearchUID(int userID)
        {

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

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

        //Only used internally
        int findNewUserID()
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

        public void LogSearchHistory(int userid, string searchvalue)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";

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
            catch
            {
            }
            return searchHistory;
        }
    }
}