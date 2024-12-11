﻿using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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

    }

    // In this function we tried using LinQ to set up the query-string. However we decided against doing it for all functions,
    // Due to it's complexit
    public Person GetPerson(string nconst)
    {

        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();

            using var cmd = new NpgsqlCommand(
                @"SELECT nconst, primaryname, birthyear, deathyear, primaryprofession
                    FROM name_basics 
                    WHERE nconst = @nconst",
                connection);

            cmd.Parameters.AddWithValue("@nconst", nconst);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var person = new Person
                {
                    Nconst = "/api/person/" + reader.GetString(0),
                    Primaryname = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Birthyear = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Deathyear = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Primaryprofessions = reader.IsDBNull(4)
                        ? null
                        : reader.GetString(4).Split(',').Select(p => new Professions { professionName = p }).ToList(),
                    KnownFor = FindKnownForTitles(nconst)
                    
                };

                Console.WriteLine($"Data Found: {person.Nconst}, {person.Primaryname}, {person.Birthyear}, {person.Deathyear}");
                return person;
            }
            else
            {
                Console.WriteLine("No person found with the given nconst.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }

        return null;
    }

    public Person createPerson(Person newPerson)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            string maxNconst = GetMaxNconst(connection);
            Console.WriteLine($"MaxNconst from DB: {maxNconst}");

            string newNconst = GenerateNextNconst(maxNconst);
            Console.WriteLine($"Generated new nconst: {newNconst}");

            using var cmd = new NpgsqlCommand("INSERT INTO name_basics (nconst, primaryname, birthyear, deathyear, primaryprofession) " +
                "VALUES (@nconst, @primaryname, @birthyear, @deathyear, @primaryprofession)" +
                "RETURNING nconst, primaryname, birthyear, deathyear, primaryprofession ;",
                connection);


            cmd.Parameters.AddWithValue("@nconst", newNconst);
            cmd.Parameters.AddWithValue("@primaryname", newPerson.Primaryname);
            cmd.Parameters.AddWithValue("@birthyear", newPerson.Birthyear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@deathyear", newPerson.Deathyear ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@primaryprofession", newPerson.Primaryprofessions != null ?
                string.Join(",", newPerson.Primaryprofessions.Select(p => p.professionName)) :
                (object)DBNull.Value);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var createdPerson = new Person
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                    Deathyear = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                    Primaryprofessions = reader.IsDBNull(4) ?
                        new List<Professions>()
                        : reader.GetString(4).Split(',').Select(p => new Professions { professionName = p }).ToList()

                };
                Console.WriteLine("Person succesfully created.");
                return createdPerson;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\nStrackTrace: {ex.StackTrace}");
        }
        return null;
    }
    
    private string GetMaxNconst(NpgsqlConnection connection)
    {
        using var cmd = new NpgsqlCommand("SELECT MAX(nconst) FROM name_basics", connection);
        object result = cmd.ExecuteScalar();
        return result != null ? result.ToString() : "nm0000000";
    }

    private string GenerateNextNconst(string maxNconst)
    {
        int numerticPart = int.Parse(maxNconst.Substring(2));

        numerticPart++;

        return "nm" + numerticPart.ToString("D7");
    }

    public IList<Person> SearchByProfession(string professionname)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            using var cmd = new NpgsqlCommand("SELECT name_basics.primaryname, name_basics.nconst, nm_professions.profession FROM name_basics INNER JOIN nm_professions ON name_basics.nconst = nm_professions.nconst WHERE nm_professions.profession = '" + professionname + "' Limit 10;", connection);

            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                var profession = new Professions()
                {
                    professionName = reader.GetString(2)
                };

                // the ProfessionList object for the person is created at filled
                // We dont fill out the nconst in the professions class, as it is already found through the Person Object
                var professionList2 = new List<Professions>
                {
                    profession
                };

                Person Person = new Person()
                {
                    Primaryname = reader.GetString(0),
                    Nconst = reader.GetString(1),
                    Primaryprofessions = professionList2
                };

                PersonList.Add(Person);

            }
        }
        catch (Exception ex)
        {
        }

        return PersonList;

    }

    public IList<Title> FindKnownForTitles(string Nconst)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Sucess\n");

            List<Title> titles = new List<Title>();
            List<Person> persons = new List<Person>();


            using (var cmd = new NpgsqlCommand("SELECT known_for.tconst, title_basics.primarytitle FROM known_for INNER JOIN title_basics ON known_for.tconst = title_basics.tconst WHERE known_for.nconst = '" + Nconst + "' ", connection))

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Title title = new Title
                    {
                        Tconst = reader.GetString(0),
                        PrimaryTitle = reader.GetString(1)
                    };
                    titles.Add(title);
                }
            }

            return titles;


            // using (var cmd = new NpgsqlCommand("SELECT primaryname FROM name_basics WHERE nconst = 'nm0001268';'"))

            /*
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

            }*/

        }

        catch
        {
            return null;
        }


    }

    public IList<Person> SearchByName(string name)
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=" + filecontent + ";Database=imdb";
        IList<Person> persons = new List<Person>();

        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Connection successful. ");

            using var cmd = new NpgsqlCommand("SELECT name_basics.nconst, name_basics.primaryname, name_basics.birthyear, name_basics.deathyear, nm_professions.profession FROM name_basics INNER JOIN nm_professions ON name_basics.nconst = nm_professions.nconst WHERE primaryname ILIKE @name", connection); // insert, primary professions and knownFor
            cmd.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var profession = new Professions()
                {
                    professionName = reader.GetString(4)
                };

                // the ProfessionList object for the person is created at filled
                // We dont fill out the nconst in the professions class, as it is already found through the Person Object
                var professionList4 = new List<Professions>
                {
                    profession
                };


                Person person = new Person()
                {
                    Nconst = reader.GetString(0),
                    Primaryname = reader.GetString(1),
                    Birthyear = reader.GetString(2),
                    Deathyear = reader.GetString(3),
                    Primaryprofessions = professionList4
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

}
