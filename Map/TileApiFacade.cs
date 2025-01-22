using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    internal class TileApiFacade
    {
        private static string API_key = "";
        private static HttpClient http = new()
        {
            BaseAddress = new Uri("https://tile.googleapis.com"),
        };
        private string sessionID;


        public TileApiFacade()
        {
            sessionID = GenerateSessionID();
        }

        private string GenerateSessionID()
        {
            //HttpContent content = new HttpContent();
            //sessionID = http.PostAsync("/v1/createSession?key={API_key}", 
            //    $"'{{\"mapType\": \"streetview\", \"language\": \"en-US\", \"region\": \"CA\"}}' " +
            //    "- H 'Content-Type: application/json");
        }
    }
}
