using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group30.Models
{
    public class Streaming_service
    {
        public string service_name;
        public bool ads;
        public string price;
        public string creation_date;
        public Streaming_service(string sn, bool a, string p, string cd)
        {
            service_name = sn; ads = a; price = p; creation_date = cd;
        }
    }

    public class Movie
    {
        public int id;
        public string title;
        public string genre;
        public int runtime;
        public bool original;
        public Movie(int id1, string title1, string genre1, int runtime1, bool original1)
        {
            id = id1; title = title1; genre = genre1; runtime = runtime1; original = original1;
        }
    }
    public class Show
    {
        public int id;
        public string title;
        public string genre;
        public int seasons;
        public int episodes;
        public bool original;
        public string channel;
        public Show(int id1, string title1, string genre1, int seasons1, int episodes1, bool original1, string channel1)
        {
            id = id1; title = title1; genre = genre1; seasons = seasons1; episodes = episodes1; original = original1; channel = channel1;
        }
    }
}
