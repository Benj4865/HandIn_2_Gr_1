using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Npgsql;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;
using HandIn_2_Gr_1.Types;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics.Metrics;


namespace HandIn_2_Gr_1;


public class DataServicePerson : IDataServicePerson
{
    public static string filepath = "C:/Users/NotAtAllPostGresPW.txt";
    public static string filecontent = File.ReadAllText(filepath);

    public IList<Person> PersonList = new List<Person>();
    public static IList<Title>? titleList = new List<Title>();



    public static void Main(string[] args)
    {
        //retrieve_data();
        //GetPerson("nm11345295");
        //DataServiceTitle.FindEpisodesFromSeriesTconst("tt0108778");

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

    public Person GetPerson(string nconst)
    {

        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear FROM name_basics WHERE nconst = '" + nconst + "' ", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Person person = new Person()
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = reader.GetString(2)
                };

                Console.WriteLine(person.Birthyear + ", " + person.Nconst + ", " + person.Primaryname);
                Console.Write("Data Found");

                return person;
            }

        }
        catch (Exception ex)
        {
        }

        return null;
    }


    public IList<Person> SearchByProfession(string professionname)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear FROM name_basics WHERE nconst = '" + professionname + "' ", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Person Person = new Person()
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = reader.GetString(2)
                };

                PersonList.Add(Person);

            }
        }
        catch (Exception ex)
        {
        }




        return null;

    }


    public static IList<Title> FindKnownForTitles(string Nconst)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            List<Title> titles = new List<Title>();
            List<Person> persons = new List<Person>();

            //= nconst needs change
            using (var cmd = new NpgsqlCommand("SELECT tb.primarytitle FROM known_for kf JOIN title_basics tb ON kf.tconst = tb.tconst WHERE kf.nconst = 'nm0000138';'" + Nconst + "' ", connection))

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Title title = new Title
                    {
                        Tconst = reader.GetString(1),
                        PrimaryTitle = reader.GetString(0)
                    };
                    titles.Add(title);

                }
            }


            using (var cmd = new NpgsqlCommand("SELECT primaryname FROM name_basics WHERE nconst = 'nm0001268';'"))


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    foreach (var title in titles)

                    {
                        Person person = new Person
                        {
                            Primaryname = reader.GetString(0),
                        };
                        persons.Add(person);

                    }
                }

            }

            return titles;

        }

        catch
        {
            return null;
        }


    }


    //måske rykke alt efer det her til en Search class
    public IList<Person> SearchByName(string name)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        IList<Person> persons = new List<Person>();

        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Connection successful. ");

            using var cmd = new NpgsqlCommand("SELECT nconst, primaryname, birthyear FROM name_basics WHERE primaryname ILIKE @name", connection);
            cmd.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Person person = new Person()
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = reader.GetString(2),
                };

                persons.Add(person);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return persons;
    }

    public IList<Title> SearchForMovieByTconst(string tconst)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            List<Title> titles = new List<Title>();

            using var cmd = new NpgsqlCommand("SELECT tconst, primarytitle FROM title_basics where tconst ='tt17156444';", connection);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Title title = new Title
                {
                    PrimaryTitle = reader.GetString(1)
                };

                titles.Add(title);

            }
            return titles;
        }

        catch
        {

        }

        return null;
       
    }

    public IList<Title> SearchByTitle(string PrimaryTitle)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            List<Title> titles = new List<Title>();

            using var cmd = new NpgsqlCommand("SELECT * FROM title_basics WHERE primarytitle = @primarytitle;", connection);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Primary title" + reader["primarytitle"]);
                Console.WriteLine("Genres" + reader["genres"]);
                Console.WriteLine("Release year" + reader["startyear"]);

            }
            
        }
        catch
        {

        }

        return null;

    }

    

    
}

// SELECT t.nconst, t.profession, s.primaryname FROM nm_professions t INNER JOIN name_basics s ON t.nconst = s.nconst where t.nconst = 'nm0006035';
// Finds name and profession from nconst