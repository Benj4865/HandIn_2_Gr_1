using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public class DataServiceTitle
    {

        public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
        public static string filecontent = File.ReadAllText(filepath);

        public static IList<Title>? titleList = new List<Title>();

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
                    Title title = new Title
                    {
                        Tconst = reader.GetString(0),
                        PrimaryTitle = reader.GetString(1),
                        IsEpisode = true
                    };

                    Console.WriteLine(title.Tconst + ", " + title.PrimaryTitle);
                    titleList.Add(title);
                   
                }
                return titleList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
                
            }

            return null;
            
        }


        public static IList<Title> TitleRatingFromTconst(string ParentTconst)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
            using var connection = new NpgsqlConnection(connectionString);

           
            IList<Title> Avgrating = new List<Title>();

            try
            {
                connection.Open();
                Console.WriteLine("Success\n");

                using var cmd = new NpgsqlCommand("SELECT tconst, averagerating from title_ratings where tconst = " + ParentTconst + "';", connection);


                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Title rating = new Title
                    {
                        Tconst = reader.GetString(0),
                        Averagerating = reader.GetDouble(1)
                    };

                    Avgrating.Add(rating);  // Use Add with a capital "A"
                }

                return Avgrating;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }

        }





    }




 

   
}
