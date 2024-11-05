using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Npgsql;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;
using HandIn_2_Gr_1.Types;


namespace HandIn_2_Gr_1;


public class DataServicePerson
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

    public static Person GetPerson(string id)
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

             cmd.Parameters.AddWithValue("nconst", "nm0000138");

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    foreach (var title in titles)

                    {
                        Person person = new Person
                        {
                            Primaryname = reader.GetString(1),
                        };
                        persons.Add(person);
                    }
                }

            }  
            
            return new { titles = titles, persons = persons };  
        }


        catch 
        {
            return null;
        }

        
    }


}

// SELECT t.nconst, t.profession, s.primaryname FROM nm_professions t INNER JOIN name_basics s ON t.nconst = s.nconst where t.nconst = 'nm0006035';
// Finds name and profession from nconst