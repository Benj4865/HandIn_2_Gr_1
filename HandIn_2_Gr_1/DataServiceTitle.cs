using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Test change
namespace HandIn_2_Gr_1
{
    internal class DataServiceTitle
    {

        public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
        public static string filecontent = File.ReadAllText(filepath);

        public static IList<Title> FindEpisodesFromSeriesTconst(string ParentTconst)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Sucess\n");


                using var cmd = new NpgsqlCommand("SELECT t.tconst, s.primarytitle FROM title_episode t INNER JOIN title_basics s ON s.tconst = t.tconst WHERE t.parenttconst = '" + ParentTconst + "';", connection);

                using var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    string tconst = reader.GetString(0);
                    string PrimaryTitle = reader.GetString(1);
                    Console.WriteLine("Tconst = " + tconst + ", Name = " + PrimaryTitle);
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }


    }
}
