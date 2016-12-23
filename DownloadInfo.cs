using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samsonkina_map
{
    public static class DownloadInfo
    {
        public class City
        {
            public string name;
            public double latitude;
            public double longitude;


            public City(string n, double lat, double lon)
            {
                name = n;
                longitude = lon;
                latitude = lat;
            }
        }

        public class Server
        {
            public string name;
            public string url;
            public string[] servers;
            public Server(string n, string u, string[] s)
            {
                name = n;
                url = u;
                servers = s;
            }
        }
    }
}
