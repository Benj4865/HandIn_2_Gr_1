using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Npgsql;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;
using HandIn_2_Gr_1.Types;


namespace HandIn_2_Gr_1;


public class DataServiceActors
{
    public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
    public static string filecontent = File.ReadAllText(filepath);

    public static void Main(string[] args)
    {
        //retrieve_data();
        GetActor("nm11345295");
    }
    public static void retrieve_data()
    {


        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");



            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear FROM name_basics WHERE nconst = 'nm11345295' ", connection);

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
    } // Can be deleted after DataService Completion

    public static Actor GetActor(string id)
    {

        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear FROM name_basics WHERE nconst = '" + id + "' ", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Actor actor = new Actor()
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = reader.GetString(2)
                };

                Console.WriteLine(actor.Birthyear + ", " + actor.Nconst + ", " + actor.Primaryname);
                Console.Write("Data Found");
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