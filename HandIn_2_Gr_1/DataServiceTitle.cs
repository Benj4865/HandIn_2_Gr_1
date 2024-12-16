using HandIn_2_Gr_1.Types;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HandIn_2_Gr_1
{
    public class DataServiceTitle : IDataServiceTitle
    {

        public static IList<Title>? titleList = new List<Title>();

        // Currently only supports 1 genre, and a new function is needed to insert into title_genre
        public Title CreateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                if (tconst.Length < 9 || tconst.Length > 10 || primaryTitle.Length < 1 || titletype.Length < 1)
                {
                    return null;
                }
                connection.Open();

                bool isAdultSQL;

                if (isAdult.ToLower() == "t")
                {
                    isAdultSQL = true;
                }
                else if (isAdult.ToLower() == "f")
                {
                    isAdultSQL = false;
                }
                else
                {
                    isAdultSQL = false;
                }

                string query = "INSERT INTO title_basics (tconst, titletype, primaryTitle, originalTitle, isAdult, startyear, endyear, runtimeMinutes, genres, poster, plot) VALUES (@tconst, @titletype, @primaryTitle, @originalTitle, @isAdult, @startyear, @endyear, @runtimeMinutes, @genres, @posterlink, @plot);";
                using var cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.AddWithValue("tconst", tconst);
                cmd.Parameters.AddWithValue("titletype", titletype);
                cmd.Parameters.AddWithValue("primaryTitle", primaryTitle);
                cmd.Parameters.AddWithValue("originalTitle", originalTitle);
                cmd.Parameters.AddWithValue("isAdult", isAdultSQL);
                cmd.Parameters.AddWithValue("startyear", startyear);
                cmd.Parameters.AddWithValue("endyear", endyear);
                cmd.Parameters.AddWithValue("runtimeMinutes", runtimeMinutes);
                cmd.Parameters.AddWithValue("genres", genres);
                cmd.Parameters.AddWithValue("posterlink", posterlink);
                cmd.Parameters.AddWithValue("plot", plot);
                cmd.ExecuteNonQuery();

                Title title = new Title
                {
                    Tconst = tconst,
                    TitleType = titletype,
                    PrimaryTitle = primaryTitle,
                    OriginalTitle = originalTitle,
                    IsAdult = isAdult,
                    StartYear = startyear,
                    EndYear = endyear,
                    RuntimeMinutes = runtimeMinutes,
                    PosterLink = posterlink,
                    plot = plot
                };

                return (title);
            }
            catch
            {
                return null;
            }
        }
        // A list of titles need to be added, so that more that one searchresult can be returned
        public IList<Title> SearchTitleByName(string name, int pageSize, int page)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                var calculatedOffSet = (page - 1) * pageSize;

                string query = "SELECT primarytitle FROM title_basics WHERE primarytitle ILIKE @searchString LIMIT @pagesize OFFSET @offset ;";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("searchString", "%" + name + "%");
                cmd.Parameters.AddWithValue("pagesize",pageSize);
                cmd.Parameters.AddWithValue("offset", calculatedOffSet);

                using var reader = cmd.ExecuteReader();

                titleList = new List<Title>();

                while (reader.Read())
                {

                    Title title = new Title
                    {
                        PrimaryTitle = reader.GetString(0)
                    };
                    
                    titleList.Add(title);
                }
                return titleList;
            }
            catch
            { }

            return null;
        }

        public Title SearchTitleByTConst(string tconst)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = "SELECT primarytitle FROM title_basics WHERE tconst = @tconst;";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("tconst",tconst);

                using var reader = cmd.ExecuteReader();

                Title title = new Title { };

                while (reader.Read())
                {
                    title.Tconst = "/api/title/" + tconst;
                    title.PrimaryTitle = reader.GetString(0);

                }
                
                return title;
            }
            catch
            { }

            return null;
        }

        // The folowing statement can not easily be scaled, but this was faster to do at the time, but had more time been given, it could have been buildt a 
        //Dynamic query that only consisted of the values that needed to be updated
        public Title updateTitle(string tconst, string titletype, string primaryTitle, string originalTitle, string isAdult, string startyear, string endyear, int runtimeMinutes, string genres, string posterlink, string plot)
        {
            var connectionString = Config.GetConnectionString();

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

        public void DeleteTitle(string tconst)
        {
            var connectionString = Config.GetConnectionString();

            using var connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();

                
                string query = "DELETE FROM title_basics WHERE tconst = @tconst;";

                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("tconst", tconst);
                
                int rowsAffected = cmd.ExecuteNonQuery();

            }
            catch
            {
            }
        }
        public bool doesTconstExist(string tconst)
        {
            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = "SELECT tconst FROM title_basics WHERE tconst = @tconst;";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("tconst", tconst);
                
                using var reader = cmd.ExecuteReader();

                reader.Read();
                var title = reader.GetString(0);

                if (title.Length >= 1)
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

        public static IList<Title> FindEpisodesFromSeriesTconst(string ParentTconst)
        {
            var connectionString = Config.GetConnectionString();
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
            var connectionString = Config.GetConnectionString();
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

        public static IList<Title> ListOftitlesBasedOnRating(IList<Title>? titleList)
        {

            var connectionString = Config.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Sucess\n");

                using var cmd = new NpgsqlCommand("SELECT tconst, averagerating FROM title_ratings WHERE numvotes >= 3070 ORDER BY averagerating DESC LIMIT 100;");

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Title title = new Title
                    {
                        Tconst = reader.GetString(0),
                        Averagerating = reader.GetDouble(1)
                    };
                    titleList.Add(title);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
            }
            return titleList;
        }
    }
}






