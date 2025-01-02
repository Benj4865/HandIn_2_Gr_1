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
using System.Buffers;



namespace HandIn_2_Gr_1;


public class DataServicePerson : IDataServicePerson
{

    public IList<Person> PersonList = new List<Person>();
    public static IList<Title>? titleList = new List<Title>();

    public static void Main(string[] args)
    {

    }

    // In this function we tried using LinQ to set up the query-string. However we decided against doing it for all functions,
    // Due to it's complexity.
    public Person GetPerson(string nconst)
    {

        var connectionString = Config.GetConnectionString();
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

    //Here we experimented with using the Person-class as input to the function, instead of passing seperate parameters.
    //This makes it more scalable. However we figured this our late in the project, and did not have time to implement it throughtout the project.
    public Person createPerson(Person newPerson)
    {
        var connectionString = Config.GetConnectionString();
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();

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

    public void DeletePerson(string nconst)
    {
        var connectionString = Config.GetConnectionString();

        using var connection = new NpgsqlConnection(connectionString);
        try
        {
            connection.Open();

            string query = "DELETE FROM name_basics WHERE nconst = @nconst;";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("nconst", nconst);

            int rowsAffected = cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
        }
    }
    public Person updatePerson(string nconst, string primaryname, string birthyear, string deathyear, string primaryprofession, string knownForTitles)
    {
        var connectionString = Config.GetConnectionString();

        using var connection = new NpgsqlConnection(connectionString);
        if (doesNconstExist(nconst))
        {
            try
            {
                connection.Open();


                if (primaryname.Length >= 1)
                {
                    string query = "UPDATE name_basics SET primaryname = @primaryname WHERE nconst = @nconst;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("nconst", nconst);
                    cmd.Parameters.AddWithValue("primaryname", primaryname);
                    cmd.ExecuteNonQuery();
                }


                if (birthyear.Length >= 1)
                {
                    string query = "UPDATE name_basics SET birthyear = @birthyear WHERE nconst = @nconst;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("nconst", nconst);
                    cmd.Parameters.AddWithValue("birthyear", birthyear);
                    cmd.ExecuteNonQuery();
                }

                if (deathyear.Length >= 1)
                {
                    string query = "UPDATE name_basics SET deathyear = @deathyear WHERE nconst = @nconst;";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("nconst", nconst);
                    cmd.Parameters.AddWithValue("deathyear", deathyear);
                    cmd.ExecuteNonQuery();
                }

                if (primaryprofession.Length >= 1)
                {
                    string query = "INSER INTO nm_professions (nconst, profession) VALUES (@nconst, primaryprofession);";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("nconst", nconst);
                    cmd.Parameters.AddWithValue("primaryprofession", primaryprofession);
                    cmd.ExecuteNonQuery();
                }

                if (knownForTitles.Length >= 1)
                {
                    string query = "INSERT INTO known_for (nconst, tconst) VALUES(@nconst, @knownfortitles);";
                    using var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("nconst", nconst);
                    cmd.Parameters.AddWithValue("knownfortitles", knownForTitles);
                    cmd.ExecuteNonQuery();
                }


                return new Person
                {
                    Nconst = nconst,
                    Primaryname = primaryname,
                    Birthyear = birthyear,
                    Deathyear = deathyear
                };
            }
            catch
            {
                return null;
            }
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

    public bool doesNconstExist(string nconst)
    {
        var connectionString = Config.GetConnectionString();
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();

            string query = "SELECT nconst FROM name_basics WHERE nconst = @nconst;";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("nconst", nconst);

            using var reader = cmd.ExecuteReader();

            reader.Read();
            var person = reader.GetString(0);

            if (person.Length >= 1)
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
            return false;
        }
    }

    public IList<Person> SearchByProfession(string professionname, int pagesize, int page)
    {
        var connectionString = Config.GetConnectionString();
        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();

            var calculatedOffSet = (page - 1) * pagesize;

            using var cmd = new NpgsqlCommand("SELECT name_basics.primaryname, name_basics.nconst, nm_professions.profession FROM name_basics INNER JOIN nm_professions ON name_basics.nconst = nm_professions.nconst WHERE nm_professions.profession = @professionname Limit @pagesize OFFSET @offset;", connection);
            cmd.Parameters.AddWithValue("professionname", professionname);
            cmd.Parameters.AddWithValue("offset", calculatedOffSet);
            cmd.Parameters.AddWithValue("pagesize", pagesize);
            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                var profession = new Professions()
                {
                    professionName = reader.GetString(2)
                };

                // the ProfessionList object for the person is created and filled
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
        catch
        {
            return null;
        }

        return PersonList;

    }

    public IList<Title> FindKnownForTitles(string Nconst)
    {
        var connectionString = Config.GetConnectionString();
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
                        Tconst = "/api/title/" + reader.GetString(0),
                        PrimaryTitle = reader.GetString(1)
                    };
                    titles.Add(title);
                }
            }

            return titles;

        }

        catch
        {
            return null;
        }


    }

    public IList<Person> SearchByName(string name, int pagesize, int page)
    {
        var connectionString = Config.GetConnectionString();
        IList<Person> persons = new List<Person>();

        using var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();

            var calculatedOffSet = (page - 1) * pagesize;

            using var cmd = new NpgsqlCommand("SELECT name_basics.nconst, name_basics.primaryname, name_basics.birthyear, name_basics.deathyear, nm_professions.profession FROM name_basics INNER JOIN nm_professions ON name_basics.nconst = nm_professions.nconst WHERE primaryname ILIKE @name LIMIT @pagesize OFFSET @offset", connection); // insert, primary professions and knownFor
            cmd.Parameters.AddWithValue("name", $"%{name}%");
            cmd.Parameters.AddWithValue("offset", calculatedOffSet);
            cmd.Parameters.AddWithValue("pagesize", pagesize);

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
                    Nconst = "/api/person/" + reader.GetString(0),
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

    public bool bookmarkPerson(string linkstring)
    {
        string nconst = "";

        try
        {
            nconst = ExtractString.extractXconst(linkstring);
        }
        catch 
        {
            Console.WriteLine("Something went wrong");
            nconst = "Error extracting nconst";
            return false;
        }

        var connectionString = Config.GetConnectionString();

        using var connection = new NpgsqlConnection(connectionString);
        try
        {
            connection.Open();
            int userID = Config.Logged_In_User();

            string query = "INSERT INTO saved_names (userid, nconst) VALUES (@userid, @nconst);";
            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("userid", userID);
            cmd.Parameters.AddWithValue("nconst", nconst);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }


}
