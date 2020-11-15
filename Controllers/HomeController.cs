using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Group30.Models;

namespace Group30.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Server=streaming-services-1.cfnlkctaecye.us-west-1.rds.amazonaws.com;Port=5432;User Id=group30;Password=xHZSAWDDQAlaAWm7qOjA;Database=postgres";
        public IActionResult Index()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM company;", conn);
            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();
            List<string> companies = new List<string>();
            while (dr.Read())
            {
                // Read all rows and output the first column in each row
                companies.Add(dr.GetString(0));
            }
            ViewData["Companies"] = companies;
            // Close connection
            conn.Close();
            return View();
        }

        public IActionResult GetStreams1(string company)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM streaming_service WHERE company_name='{company}';", conn);
            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();
            List<Streaming_service> services = new List<Streaming_service>();
            while (dr.Read())
            {
                services.Add(new Streaming_service(dr.GetString(1), dr.GetBoolean(2), dr.GetDecimal(3).ToString(), dr.GetDate(4).ToString()));
            }
            conn.Close();
            return Ok(services);
        }

        public List<Movie> GetMovies(string company, string ss)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string command1 = $@"SELECT s.media_id, med.media_name, med.original, med.genre, mov.runtime 
                                FROM streams s, media med, movie mov 
                                WHERE s.company_name = '{company}' AND s.service_name = '{ss}' AND s.media_id = med.media_id AND med.media_id = mov.media_id; ";
            NpgsqlCommand cmd1 = new NpgsqlCommand(command1, conn);
            // Execute a query
            NpgsqlDataReader dr = cmd1.ExecuteReader();
            List<Movie> ml1 = new List<Movie>();
            while (dr.Read())
            {
                ml1.Add(new Movie(dr.GetInt32(0), dr.GetString(1), dr.GetString(3), dr.GetInt32(4), dr.GetBoolean(2)));
            }
            conn.Close();
            return ml1;
        }

        public List<Show> GetShows(string company, string ss)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string command1 = $@"SELECT s.media_id, med.media_name, med.original, med.genre, sh.episodes, sh.seasons, sh.channel
                                FROM streams s, media med, series sh
                                WHERE s.company_name = '{company}' AND s.service_name = '{ss}' AND s.media_id = med.media_id AND med.media_id = sh.media_id;";
            NpgsqlCommand cmd1 = new NpgsqlCommand(command1, conn);
            // Execute a query
            NpgsqlDataReader dr = cmd1.ExecuteReader();
            List<Show> sl1 = new List<Show>();
            while (dr.Read())
            {
                sl1.Add(new Show(dr.GetInt32(0), dr.GetString(1), dr.GetString(3), dr.GetInt32(5), dr.GetInt32(4), dr.GetBoolean(2), dr.GetString(6)));
            }
            conn.Close();
            return sl1;
        }

        public IActionResult GetMovieLists(string company1, string service1, string company2, string service2)
        {
            List<List<Movie>> movieLists = new List<List<Movie>>();
            movieLists.Add(GetMovies(company1, service1));
            movieLists.Add(GetMovies(company2, service2));
            return Ok(movieLists);
        }

        public IActionResult GetShowLists(string company1, string service1, string company2, string service2)
        {
            List<List<Show>> showLists = new List<List<Show>>();
            showLists.Add(GetShows(company1, service1));
            showLists.Add(GetShows(company2, service2));
            return Ok(showLists);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
