using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
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

                    return UserList;
                }

            }
            catch (Exception ex)
            {
            }

            return null;


        }
       public IList<User> SearchUser(string username, string useremail)
        {
            //Define the connection string with PostgreSQL credentials and database name.
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
            
            //Creates list to hold the found users.
            IList<User> foundUsers = new List<User>();

            //Initializing a connections with the database.
            using var connection = new NpgsqlConnection( connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Database connection succesful! ");

                //SQL query with parameter placeholder| ILIKE used for case insensitive and % for wildcard
                string query = "SELECT username, useremail FROM Users WHERE username ILIKE @SearchTerm or useremail ILIKE @SearchTerm;" ;
                
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return foundUsers;

        }
    }
}
