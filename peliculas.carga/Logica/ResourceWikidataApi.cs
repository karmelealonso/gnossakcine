using Gnoss.ApiWrapper.ApiModel;
using Newtonsoft.Json;
using peliculas.carga.DTO;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace peliculas.carga.Logica
{
    class ResourceWikidataApi
    {

        WebClient _webClient;

        public ResourceWikidataApi() {
            _webClient = new();
            _webClient.Encoding = Encoding.UTF8;
            _webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            _webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            _webClient.Headers.Add("accept", "application/sparql-results+json");
        }

        public SparqlObject hacerPeticionAPI(string sparqlQuery, string idLog)
        {
            SparqlObject datos = new SparqlObject();
            System.Collections.Specialized.NameValueCollection parametros = new
                System.Collections.Specialized.NameValueCollection
            {
                { "query", sparqlQuery.ToString() }
            };
            byte[] responseArray = null;
            int numIntentos = 0;
            string error = "";
            while (responseArray == null && numIntentos < 5)
            {
                numIntentos++;
                try
                {
                    responseArray = _webClient.UploadValues("https://query.wikidata.org/sparql", "POST", parametros);                    
                    if (responseArray is null) {
                        Console.WriteLine($"Para {idLog} no se han obtenido datos de WIKIDATA");
                        return datos;                     
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }

            string jsonRespuesta = System.Text.Encoding.UTF8.GetString(responseArray);
            if (!string.IsNullOrEmpty(jsonRespuesta))
            {
                datos = JsonConvert.DeserializeObject<SparqlObject>(jsonRespuesta);
            }
            return datos;
        }
    }
}
