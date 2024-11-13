// See https://aka.ms/new-console-template for more information
using peliculas.carga.DTO;
using peliculas.carga.Logica;
using GenerokarmeleOntology;
using PersonakarmeleOntology;
using PeliculakarmeleOntology;
using Gnoss.ApiWrapper;

using System.Text.RegularExpressions;
using peliculas.carga.Logica;



namespace peliculas.carga
{
    class Program
    {
        static void Main(string[] args)
        {
            //REGIÓN CONEXIÓN Y DATOS DE LA COMUNIDAD
            int numPeliculasCargar = 100;

            string pathOAuth = @"Config\oAuth.config";
            ResourceApi mResourceApi = new ResourceApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));

            //List<string> recursosSecundariosBorrar = new List<string>() { "", "" };

            //mResourceApi.DeleteSecondaryEntitiesList(ref recursosSecundariosBorrar);


            Lector lector = new Lector();
            Cargador cargador = new Cargador();
            List<PeliculaDTO> l_PeliculaDTO = lector.LeerPeliculas(numPeliculasCargar);

            // Inicializamos los HashSets para guardar nombres únicos
            HashSet<string> hs_nombreOcupaciones = new HashSet<string>()
            {
                "screenwriter", "actor", "director"
            };                 

            HashSet<string> hs_nombrePersonas = new HashSet<string>();
            HashSet<string> hs_nombreGeneros = new HashSet<string>();
            foreach (var peliculaDTO in l_PeliculaDTO)
            {
                hs_nombreOcupaciones = Logica.Utils.SepararGeneros(peliculaDTO.Genre, hs_nombreGeneros);
                hs_nombreGeneros = Logica.Utils.SepararGeneros(peliculaDTO.Genre, hs_nombreGeneros);
                hs_nombrePersonas = Logica.Utils.SepararPersonas(peliculaDTO, hs_nombrePersonas);
            }

            // Cargar la lista de géneros
            List<Genre> l_genre = cargador.CargarGeneros(mResourceApi, hs_nombreGeneros);

            // Cargar la lista de personas
            List<Person> l_personas = cargador.CargarPersonas(mResourceApi, hs_nombrePersonas, l_PeliculaDTO);

            // Cargar la lista de películas
            List<Movie> peliculasCargadas = cargador.CargarPeliculas(mResourceApi, l_PeliculaDTO);
        }
    }
}


