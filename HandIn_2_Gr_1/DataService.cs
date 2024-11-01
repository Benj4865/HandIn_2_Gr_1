using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Npgsql;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;


namespace HandIn_2_Gr_1;


public class DataService
{
    public static string filepath = "C:/Users/bena3/Desktop/NotAtAllPostGresPW.txt";
    public static string filecontent = File.ReadAllText(filepath);

    public static void Main(string[] args)
    {
        retrieve_data();
    }
    public static void retrieve_data()
    {
        

        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent+ ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            

            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear  FROM name_basics WHERE nconst = 'nm11345295' ", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                string tconst = reader.GetString(0);
                string nconst = reader.GetString(1);
                Console.WriteLine("Tconst = " + tconst + ", Nconst = " + nconst);
            }
        }
        catch (Exception ex)
        {

        }
    }

    public Actor GetActor(string id)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            using var cmd = new NpgsqlCommand("SELECT tconst, nconst  FROM known_for WHERE nconst = 'nm0006535' ", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                string tconst = reader.GetString(0);
                string nconst = reader.GetString(1);
                Console.WriteLine("Tconst = " + tconst + ", Nconst = " + nconst);
            }
        }
        catch (Exception ex)
        {

        }

        return null;
    }



    public IList<Actor> SearchByProfession(string professionname)
    {


        return null;

    }


}