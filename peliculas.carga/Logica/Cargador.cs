using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeliculakarmeleOntology;
using PersonakarmeleOntology;
using GenerokarmeleOntology;
using OcupacionkarmeleOntology;
using PremiokarmeleOntology;
using peliculas.carga.DTO;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;
using System.Globalization;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using Gnoss.ApiWrapper.Helpers;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;



namespace peliculas.carga.Logica
{
    internal class Cargador
    {
        public Cargador()
        {

        }

        public List<Genre> CargarGeneros(ResourceApi mResourceApi, HashSet<string> l_nombreGeneros)
        {
            List<Genre> l_Genre = new List<Genre>();

            // Cambio de ontología a géneros
            mResourceApi.ChangeOntology("generokarmele.owl");

            // Se obtienen todos los nombres de los géneros cargados en el Grafo
            List<Tuple<string, string>> t_URIsNombresExistentes = ObtenerRecursos(mResourceApi, "generokarmele");

            List<string> nombresExistentes = t_URIsNombresExistentes.Select(t => t.Item2).Where(nombre => !nombre.Trim().ToLower().Equals(("N/A").ToLower())).ToList();

            List<string> generosParaCargar = l_nombreGeneros.Where(nombregenerodatos => !(nombresExistentes.Contains(nombregenerodatos))).ToList();

            foreach (var nombreGeneroCargar in generosParaCargar)
            {
                if (nombreGeneroCargar.Trim().ToLower().Equals(("N/A").ToLower())) { continue; }

                string identificador = Guid.NewGuid().ToString(); // Se genera un identificador único
                Genre genreKG = new Genre(identificador);

                genreKG.Schema_name = nombreGeneroCargar; // Asignamos el nombre del género al schema

                SecondaryResource generoSR = genreKG.ToGnossApiResource(mResourceApi, $"Genre_{identificador}");

                string mensajeFalloCarga = $"Error en la carga del Género con identificador {identificador} -> Nombre: {genreKG.Schema_name}";

                try
                {
                    mResourceApi.LoadSecondaryResource(generoSR);
                    if (!generoSR.Uploaded)
                    {
                        mResourceApi.Log.Error(mensajeFalloCarga);
                    }
                }
                catch (Exception ex)
                {
                    mResourceApi.Log.Error($"Exception -> {mensajeFalloCarga}: {ex.Message}");
                }
                l_Genre.Add(genreKG);
            }
            return l_Genre;
        }

        /// <summary>
        /// Método que carga todas los premios en el KG
        /// </summary>
        /// <param name="mResourceApi"></param>
        /// <param name="l_nombrePersonas"></param>
        /// <returns></returns>
        /// 

        // Método para obtener el nombre del premio padre de WikiData
        public static PremioDTOWikidata ObtenerDatosPremioDesdeWikidata(string nombrePremio)
        {
            PremioDTOWikidata premioDTO = new PremioDTOWikidata();

            string lineaConsulta = "";
            if (nombrePremio.Contains("\""))
            {
                lineaConsulta = "?premio rdfs:label '" + nombrePremio + @"'@en. ";
            }
            else
            {
                lineaConsulta = @"?premio rdfs:label """ + nombrePremio + @"""@en.";
            }

            // Definir la consulta SPARQL para buscar el premio por su nombre
            string query = $@"
            SELECT ?premio ?nombrePremio ?categoria 
            WHERE {{
                {lineaConsulta}
                ?premio rdfs:label ?nombrePremio. # Busca premio
                ?premio wdt:P31 ?tipoPremio.
                #?tipoPremio wdt:P31 wd:Q107655869.
                ?tipoPremio rdfs:label ?nombreTipoPremio.
                FILTER(LANG(?nombrePremio) = ""en"")
                FILTER(LANG(?nombreTipoPremio) = ""en"")
                OPTIONAL {{
                    ?premio wdt:P361 ?padrePremio.
                    ?padrePremio rdfs:label ?nombrePadrePremio. # Asegura que etiquetas del padre se relacionan
                    FILTER(LANG(?nombrePadrePremio) = ""en"")
                }}
                BIND(IF(BOUND(?nombrePadrePremio), ?nombrePadrePremio, ?nombreTipoPremio) AS ?categoria)            
            }} LIMIT 1";

            ResourceWikidataApi mResourceWDAPI = new ResourceWikidataApi();
            SparqlObject datos = mResourceWDAPI.hacerPeticionAPI(query, nombrePremio);

            // Procesar resultados si existen
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                var binding = datos.results.bindings.FirstOrDefault();

                // Asignar valores al objeto PremioDTO
                if (binding.ContainsKey("premio"))
                {
                    premioDTO.UriPremio = binding["premio"].value;
                }
                if (binding.ContainsKey("nombrePremio"))
                {
                    // premioDTO.NombrePremio = binding["nombrePremio"].value;
                    premioDTO.NombrePremio = nombrePremio;
                }
                if (binding.ContainsKey("nombreTipoPremio"))
                {
                    premioDTO.NombreTipoPremio = binding["nombreTipoPremio"].value;
                }
                //if (binding.ContainsKey("padrePremio"))
                //{
                //    premioDTO.UriPadrePremio = binding["padrePremio"].value;
                //}
                //if (binding.ContainsKey("nombrePadrePremio"))
                //{
                //    premioDTO.NombrePadrePremio = binding["nombrePadrePremio"].value;
                //}
                if (binding.ContainsKey("categoria"))
                {
                    premioDTO.Categoria = binding["categoria"].value;
                }
            }
            return premioDTO;
        }

        /// <summary>
        /// Método que carga todos los premios en el KG
        /// </summary>
        /// <param name="mResourceApi"></param>
        /// <param name="l_nombrePremios"></param>
        /// <returns></returns>

        public List<Tuple<string, string>> CargarPremios(ResourceApi mResourceApi, List<string> l_nombrePremios)
        {            
            // Cambio de ontología a premios
            mResourceApi.ChangeOntology("premiokarmele.owl");

            // Obtener todos los nombres de premios ya cargados en el grafo
            List<Tuple<string, string>> t_URIsNombresExistentes = ObtenerRecursos(mResourceApi, "premiokarmele");

            List<string> nombresExistentes = t_URIsNombresExistentes.Select(t => t.Item2).Where(nombre => l_nombrePremios.Contains(nombre)).ToList();

            // Filtrar nombres de premios que ya están cargados (para asignarselos a las personas)
            List<Tuple<string, string>> t_URIsNombresPremiosExistentes = t_URIsNombresExistentes.Where(nombre => nombresExistentes.Contains(nombre.Item2)).ToList();

            // Filtrar nombres de premios que aún no están cargados
            List<string> premiosParaCargar = l_nombrePremios.Where(nombrePremio => !nombresExistentes.Contains(nombrePremio)).ToList();

            foreach (var nombrePremio in premiosParaCargar)
            {
                // Generar un identificador único
                string identificador = Guid.NewGuid().ToString();

                Awards awardsKG = new Awards { Schema_name = nombrePremio };

                // Obtener datos adicionales del premio desde Wikidata
                PremioDTOWikidata premioDTOWikidata = ObtenerDatosPremioDesdeWikidata(nombrePremio);

                // Asignar propiedades basadas en los datos obtenidos de Wikidata
                if (!string.IsNullOrEmpty(premioDTOWikidata.NombrePremio))
                {
                    awardsKG.Schema_name = premioDTOWikidata.NombrePremio;
                }


                // Validar la categoría para determinar si necesita asignarse a "Other awards"
                if (!string.IsNullOrEmpty(premioDTOWikidata.Categoria))
                {
                    // Verificar si la categoría contiene al menos una letra mayúscula
                    if (Regex.IsMatch(premioDTOWikidata.Categoria, "[A-Z]"))
                    {
                        awardsKG.Schema_category = premioDTOWikidata.Categoria;
                    }
                    else
                    {
                        awardsKG.Schema_category = "Other awards"; // Asignar categoría predeterminada
                    }
                }
                else
                {
                    awardsKG.Schema_category = "Other awards"; // Categoría predeterminada si está vacía
                }

                //if (!string.IsNullOrEmpty(premioDTOWikidata.Categoria))
                //{
                //    awardsKG.Schema_category = premioDTOWikidata.Categoria;
                //}

                ComplexOntologyResource awardsSR = awardsKG.ToGnossApiResource(mResourceApi, new List<string>(), Guid.NewGuid(), Guid.NewGuid());
                try
                {
                    mResourceApi.LoadComplexSemanticResource(awardsSR);
                    if (!awardsSR.Uploaded)
                    {
                        mResourceApi.Log.Error($"Error en la carga del premio con identificador {identificador} -> Nombre: {awardsKG.Schema_name}");
                    }
                }
                catch (Exception ex)
                {
                    mResourceApi.Log.Error($"Exception -> Error en la carga del premio con identificador {identificador}: {ex.Message}");
                }

                // Añadir la URI y el nombre del premio a la lista de tuplas
                t_URIsNombresPremiosExistentes.Add(new Tuple<string, string>(awardsSR.GnossId, awardsKG.Schema_name));
            }

            return t_URIsNombresPremiosExistentes;
        }

        /// <summary>
        /// Método que se encarga de cargar todas las ocupaciones de una persona si no están en el KG
        /// </summary>
        /// <param name="mResourceApi"></param>
        /// <param name="hs_nombreOcupaciones"></param>
        /// <returns>Lista de tuplas con las ocupaciones existentes en el KG después de actualizarlo con las ocupaciones de la persona <URI, nombre></returns>
        public List<Tuple<string, string>> CargarOcupaciones(ResourceApi mResourceApi, List<string> hs_nombreOcupaciones)
        {
            List<Occupation> l_Occupation = new List<Occupation>();

            // Cambio de ontología a ocupaciones
            mResourceApi.ChangeOntology("ocupacionkarmele.owl");

            // Se obtienen todos los nombres de los ocupaciones cargados en el Grafo
            List<Tuple<string, string>> t_URIsNombresExistentes = ObtenerRecursos(mResourceApi, "ocupacionkarmele");

            List<string> nombresExistentes = t_URIsNombresExistentes.Select(t => t.Item2).Where(nombre => !nombre.Trim().ToLower().Equals(("N/A").ToLower())).ToList();

            List<string> ocupacionesParaCargar = hs_nombreOcupaciones.Where(nombreocupaciondatos => !(nombresExistentes.Contains(nombreocupaciondatos))).ToList();

            foreach (var nombreOcupacionCargar in ocupacionesParaCargar)
            {
                
                string identificador = Guid.NewGuid().ToString(); // Se genera un identificador único
                Occupation occupationKG = new Occupation(identificador);

                occupationKG.Schema_name = nombreOcupacionCargar; // Asignamos el nombre de la ocupación al schema

                SecondaryResource ocupacionSR = occupationKG.ToGnossApiResource(mResourceApi, $"Occupation_{identificador}");

                string mensajeFalloCarga = $"Error en la carga de la Ocupación con identificador {identificador} -> Nombre: {occupationKG.Schema_name}";

                try
                {
                    mResourceApi.LoadSecondaryResource(ocupacionSR);
                    if (!ocupacionSR.Uploaded)
                    {
                        mResourceApi.Log.Error(mensajeFalloCarga);
                    }
                    else
                    {
                        Tuple<string,string> t_ocupacion = new Tuple<string, string>(ocupacionSR.Id, occupationKG.Schema_name);
                        t_URIsNombresExistentes.Add(t_ocupacion);
                    }
                }
                catch (Exception ex)
                {
                    mResourceApi.Log.Error($"Exception -> {mensajeFalloCarga}: {ex.Message}");
                }
                l_Occupation.Add(occupationKG);
            }
            return t_URIsNombresExistentes;
        }


        // Método para obtener datos de Wikidata
        public static PersonaDTOWikidata? ObtenerDatosPersonaDesdeWikidata(string nombre)
        {
            PersonaDTOWikidata persona = new PersonaDTOWikidata();








            // Definir la consulta SPARQL
            String select = $@"SELECT ?person ?image ?sexOrGenderLabel ?countryOfCitizenshipLabel ?dateOfBirthLabel ?placeOfBirthLabel 
            (GROUP_CONCAT(DISTINCT(?occupationName) ; separator = "" || "") AS ?occupationLabelConcat)
            (GROUP_CONCAT(DISTINCT(?notableWorkName) ; separator = "" || "") AS ?notableWorkLabelConcat)
            (GROUP_CONCAT(DISTINCT(?awardName) ; separator = "" || "") AS ?awardReceivedLabelConcat)";
            String where = $@"
            WHERE {{
                ?person rdfs:label ""{nombre}""@en;  
                        wdt:P31 wd:Q5;                      # Instancia de humano
                        wdt:P106 ?occupation.               # Ocupación

                # Filtrar ocupaciones 
                VALUES ?occupation {{ 
                    wd:Q33999         # Actor
                    wd:Q2526255       # Director de cine
                    wd:Q28389         # Guionista
                    wd:Q6625963       # Novelista
                    wd:Q36180         # Escritor
                    wd:Q482980        # Ensayista
                    wd:Q11774202      # Escritor de literatura infantil
                    wd:Q333634        # Traductor
                    wd:Q214917        # Crítico literario
                    wd:Q49757         # Poeta      
                    wd:Q1028181       # Ilustrador
                    wd:Q37226         # Profesor
                    wd:Q214917        # Escritor de prosa
                }}
  
                # Extraer propiedades 
                
                ?occupation rdfs:label ?occupationName.
                            FILTER(LANG(?occupationName) = ""en"")          
                OPTIONAL {{ ?person wdt:P18 ?image. }}                    # Imagen
                OPTIONAL {{ ?person wdt:P21 ?sexOrGender. }}              # Sexo o género
                OPTIONAL {{ ?person wdt:P27 ?countryOfCitizenship. }}     # País de ciudadanía
                OPTIONAL {{ ?person wdt:P569 ?dateOfBirth. }}             # Fecha de nacimiento
                OPTIONAL {{ ?person wdt:P19 ?placeOfBirth. }}             # Lugar de nacimiento
                OPTIONAL {{ ?person wdt:P800 ?notableWork.
                            ?notableWork rdfs:label ?notableWorkName.
                                         FILTER(LANG(?notableWorkName) = ""en"")        # Obra notable
                         }}                                                         
                OPTIONAL {{ ?person wdt:P166 ?awardReceived.
                            ?awardReceived rdfs:label ?awardName.
                                          FILTER(LANG(?awardName) = ""en"")     # Premios recibidos

                         }}           

                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}   
            }}
            GROUP BY ?person ?image ?dateOfBirthLabel ?sexOrGenderLabel ?countryOfCitizenshipLabel ?placeOfBirthLabel";

            string sparqlQuery = select + where;

            ResourceWikidataApi mResourceWDAPI = new ResourceWikidataApi();
            SparqlObject datos = mResourceWDAPI.hacerPeticionAPI( sparqlQuery,  nombre);

            // Inicializar valores
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                var binding = datos.results.bindings.FirstOrDefault();

                // Asignar valores a la persona
                if (binding.ContainsKey("image"))
                {
                    persona.Image = binding["image"].value;
                }
                if (binding.ContainsKey("sexOrGenderLabel"))
                {
                    persona.SexOrGender = binding["sexOrGenderLabel"].value;
                }
                if (binding.ContainsKey("countryOfCitizenshipLabel"))
                {
                    persona.CountryOfCitizenship = binding["countryOfCitizenshipLabel"].value;
                }
                if (binding.ContainsKey("dateOfBirthLabel"))
                {
                    string fecha = binding["dateOfBirthLabel"].value;
                    DateTime fechaConvertida = new DateTime();
                    if (DateTime.TryParseExact(fecha, "d 'de' MMMM 'de' yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaConvertida)) 
                    { 
                        persona.DateOfBirth = fechaConvertida;
                    }
                    else if (DateTime.TryParseExact(fecha, "d 'de' MM 'de' yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaConvertida))
                    {
                        persona.DateOfBirth = fechaConvertida;
                    }                    
                    else
                    {
                        string patron = @"\d{4}";

                        MatchCollection coincidencias = Regex.Matches(fecha, patron);

                        if (coincidencias.Count == 1)
                        {
                            int.TryParse(coincidencias.First().ToString(), out int anio);
                            persona.DateOfBirth = new DateTime(anio,1,1);
                        }                            
                    }
                }
                if (binding.ContainsKey("placeOfBirthLabel"))
                {
                    persona.PlaceOfBirth = binding["placeOfBirthLabel"].value;
                }
                
                if (binding.ContainsKey("occupationLabelConcat")) 
                {
                    persona.Occupation = binding["occupationLabelConcat"].value.Split(" || ").ToList();
                }
                if (binding.ContainsKey("notableWorkLabelConcat"))
                {
                    persona.NotableWork = binding["notableWorkLabelConcat"].value.Split(" || ").ToList();
                }
                if (binding.ContainsKey("awardReceivedLabelConcat") && !string.IsNullOrEmpty(binding["awardReceivedLabelConcat"].value))
                {
                    persona.AwardReceived = binding["awardReceivedLabelConcat"].value.Split(" || ").ToList();
                }

            }

            return persona;
        }


            /// <summary>
            /// Método que carga todas las personas en el KG
            /// </summary>
            /// <param name="mResourceApi"></param>
            /// <param name="l_nombrePersonas"></param>
            /// <returns></returns>
            public List<Person> CargarPersonas(ResourceApi mResourceApi, HashSet<string> l_nombrePersonas, List<PeliculaDTO> l_PeliculaDTO)
            {
                List<Person> l_Person = new List<Person>();

                // Cambio de ontología a personas
                mResourceApi.ChangeOntology("personakarmele.owl");

                // Se obtienen todos los nombres de las personas cargadas en el Grafo
                List<Tuple<string, string>> t_URIsNombresExistentes = ObtenerRecursos(mResourceApi, "personakarmele");
                List<string> nombresExistentes = t_URIsNombresExistentes.Select(t => t.Item2).ToList();

                List<string> l_nombrePersonas_Cargar = l_nombrePersonas.Where(pelicula => !nombresExistentes.Contains(pelicula)).ToList();

                foreach (var nombrePersona in l_nombrePersonas_Cargar)
                {

                    string identificador = Guid.NewGuid().ToString(); // Generar un identificador único
                    Person personaKG = new Person { Schema_name = nombrePersona }; // Asignar el nombre de la persona

                    PersonaDTOWikidata personaDTOWikidata = ObtenerDatosPersonaDesdeWikidata(nombrePersona);
         

                // Asignar propiedades a la nueva clase
          

                if (!string.IsNullOrEmpty(personaDTOWikidata.Image))
                {
                    personaKG.Schema_image =  personaDTOWikidata.Image ;
                }

                if (!string.IsNullOrEmpty(personaDTOWikidata.SexOrGender))
                {
                    personaKG.Schema_gender =  personaDTOWikidata.SexOrGender ;
                }

                if (personaDTOWikidata.DateOfBirth is not null)
                {
                    personaKG.Schema_birthDate = personaDTOWikidata.DateOfBirth;
                    string añoComoTexto = personaDTOWikidata.DateOfBirth.ToString(); // Obtiene el año como una cadena
                    personaKG.Schema_startDate = int.Parse(añoComoTexto.Substring(6, 4)); // Convierte la cadena a un entero si es necesari            
                }
         
                if (!string.IsNullOrEmpty(personaDTOWikidata.PlaceOfBirth))
                {
                    personaKG.Schema_birthPlace =  personaDTOWikidata.PlaceOfBirth;
                }

                if (personaDTOWikidata.NotableWork != null && personaDTOWikidata.NotableWork.Count > 0)
                {
                    personaKG.Schema_CreativeWork = personaDTOWikidata.NotableWork; 
                }

                if (personaDTOWikidata.Occupation != null && personaDTOWikidata.Occupation.Count > 0)
                {
                    List<Tuple<string, string>> t_URIsNombresExistentesOcupaciones =  CargarOcupaciones(mResourceApi, personaDTOWikidata.Occupation);
                    personaKG.IdsSchema_hasOccupation = new List<string>();

                    //JSON

                    bool isActor = false;
                    bool isDirector = false;
                    bool isGuionista = false;
                    foreach (var peliculaDTO in l_PeliculaDTO)
                    {
                        if (!isActor)
                        {
                            HashSet<string> actoresEnPelicula = new HashSet<string>();
                            actoresEnPelicula = peliculas.carga.Logica.Utils.SepararTipoPersonas(peliculaDTO.Actors, actoresEnPelicula);
                            isActor = actoresEnPelicula.Contains(nombrePersona);
                        }

                        if (!isDirector)
                        {
                            HashSet<string> directoresEnPelicula = new HashSet<string>();
                            directoresEnPelicula = peliculas.carga.Logica.Utils.SepararTipoPersonas(peliculaDTO.Director, directoresEnPelicula);
                            isDirector = directoresEnPelicula.Contains(nombrePersona);
                        }

                        if (!isGuionista)
                        {
                            HashSet<string> guionistasEnPelicula = new HashSet<string>();
                            guionistasEnPelicula = peliculas.carga.Logica.Utils.SepararTipoPersonas(peliculaDTO.Writer, guionistasEnPelicula);
                            isGuionista = guionistasEnPelicula.Contains(nombrePersona);
                        }
                    }
                    string uriActor = "http://gnoss.com/items/Occupation_b43066d9-ab83-4fd4-88a7-efc32d650050";
                    string uriDirector = "http://gnoss.com/items/Occupation_d21c58c5-f159-4438-8362-60ec4d521ac6";
                    string uriGuionista = "http://gnoss.com/items/Occupation_7e9620e8-bed3-44f5-8caf-e70727018950";

                    if (isActor) personaKG.IdsSchema_hasOccupation.Add(uriActor);
                    if (isDirector) personaKG.IdsSchema_hasOccupation.Add(uriDirector);
                    if (isGuionista) personaKG.IdsSchema_hasOccupation.Add(uriGuionista);

                    //WIKIDATA
                    foreach (var nombreOcupacion in personaDTOWikidata.Occupation)
                    {
                        string uriOcupacion = "";
                        uriOcupacion = t_URIsNombresExistentesOcupaciones.FirstOrDefault(tupla => tupla.Item2 == nombreOcupacion).Item1;
                        if (!personaKG.IdsSchema_hasOccupation.Contains(uriOcupacion))
                        {
                            personaKG.IdsSchema_hasOccupation.Add(uriOcupacion);
                        }                 
                    }
                }

                if (!string.IsNullOrEmpty(personaDTOWikidata.CountryOfCitizenship))
                {
                    personaKG.Schema_nationality =  personaDTOWikidata.CountryOfCitizenship ;
                }



                if (personaDTOWikidata.AwardReceived != null && personaDTOWikidata.AwardReceived.Count > 0)
                {
                    List<Tuple<string, string>> t_URIsNombresExistentesPremios = CargarPremios(mResourceApi, personaDTOWikidata.AwardReceived);
                    personaKG.IdsSchema_awards = new List<string>();

                    //WIKIDATA
                    foreach (var nombrePremio in personaDTOWikidata.AwardReceived)
                    {
                        string uriPremio = "";
                        uriPremio = t_URIsNombresExistentesPremios.FirstOrDefault(tupla => tupla.Item2 == nombrePremio)?.Item1;
                        if (!string.IsNullOrEmpty(uriPremio) && !personaKG.IdsSchema_awards.Contains(uriPremio))
                        {
                            personaKG.IdsSchema_awards.Add(uriPremio);
                        } else
                        {
                            Console.WriteLine();

                        }
                    }
                }


                // Crear recurso ontológico para la persona
                mResourceApi.ChangeOntology("personakarmele.owl");
                ComplexOntologyResource personaSR = personaKG.ToGnossApiResource(mResourceApi, new List<string>(), Guid.NewGuid(), Guid.NewGuid());

                    try
                    {
                        mResourceApi.LoadComplexSemanticResource(personaSR);
                        if (!personaSR.Uploaded)
                        {
                            mResourceApi.Log.Error($"Error en la carga de la persona con identificador {identificador} -> Nombre: {personaKG.Schema_name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        mResourceApi.Log.Error($"Exception -> Error en la carga de la persona con identificador {identificador}: {ex.Message}");
                    }

                    l_Person.Add(personaKG);  // Añadir la persona cargada a la lista
                }

                return l_Person;
            }

            public List<Movie> CargarPeliculas(ResourceApi mResourceApi, List<PeliculaDTO> l_PeliculaDTO)
            {
                List<Movie> l_Movie = new List<Movie>();

                // Cambio de ontología a películas
                mResourceApi.ChangeOntology("peliculakarmele.owl");

                // Se obtienen todos los nombres de las películas cargadas en el Grafo
                List<Tuple<string, string>> t_URIsNombresPeliculasExistentes = ObtenerRecursos(mResourceApi, "peliculakarmele");
                List<Tuple<string, string>> t_URIsNombresPersonasExistentes = ObtenerRecursos(mResourceApi, "personakarmele");
                List<Tuple<string, string>> t_URIsNombresGenerosExistentes = ObtenerRecursos(mResourceApi, "generokarmele");

                List<string> titulosPeliculasExistentes = t_URIsNombresPeliculasExistentes.Select(t => t.Item2).ToList();

                List<PeliculaDTO> l_PeliculaCargarDTO = l_PeliculaDTO.Where(peliculaDTO => !(titulosPeliculasExistentes.Contains(peliculaDTO.Title))).ToList();

                foreach (var peliculaDTO in l_PeliculaCargarDTO)
                {

                    // Validar que la película tenga imagen antes de procesarla
                    //if (string.IsNullOrEmpty(peliculaDTO.Poster))
                    //{
                    //    // Si no tiene imagen, se omite y se pasa a la siguiente
                    //    continue;
                    //}

                    Movie pelicula = new Movie();

                    //Título, Descripción e Imagen
                    pelicula.Schema_name = peliculaDTO.Title;
                    pelicula.Schema_description = peliculaDTO.Plot;


                    if (string.IsNullOrEmpty(peliculaDTO.Poster) || peliculaDTO.Poster.Trim().Length == 0 || !Uri.IsWellFormedUriString(peliculaDTO.Poster, UriKind.Absolute) || peliculaDTO.Poster.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_image = "https://www.lafalda.gob.ar/sites/default/files/base_hombre.jpg";
                    }
                    else
                    {
                        pelicula.Schema_image = peliculaDTO.Poster;
                    }


                    //Productora
                    if (!string.IsNullOrEmpty(peliculaDTO.Production) && !peliculaDTO.Production.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_productionCompany = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, List<string>>()
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, new List<string> { peliculaDTO.Production.Trim() } }
                    };
                    }


                    //Año
                    if (!string.IsNullOrEmpty(peliculaDTO.Year) && !peliculaDTO.Year.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_recordedAt = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, List<string>>()
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, new List<string> { peliculaDTO.Year } }
                    };
                    }


                    //Países


                    if (!string.IsNullOrEmpty(peliculaDTO.Country) && !peliculaDTO.Country.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        string[] paises = peliculaDTO.Country.Split(',');
                        foreach (var pais in paises)
                        {
                            pelicula.Schema_countryOfOrigin = pais.Trim();
                        }
                    }


                    // Duración
                    if (!string.IsNullOrEmpty(peliculaDTO.Runtime) && !peliculaDTO.Runtime.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_duration = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, peliculaDTO.Runtime }
                    };
                    }

                    pelicula.Schema_inLanguage = new();
                    //Idiomas
                    if (!string.IsNullOrEmpty(peliculaDTO.Language) && !peliculaDTO.Language.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        string[] idiomas = peliculaDTO.Language.Split(',');
                        foreach (var idioma in idiomas)
                        {
                            pelicula.Schema_inLanguage.Add(idioma.Trim());
                        }
                    }


                    //Premios
                    if (!string.IsNullOrEmpty(peliculaDTO.Awards) && !peliculaDTO.Awards.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_award = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, List<string>>()
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, new List<string> { peliculaDTO.Awards.Trim() } }
                    };
                    }

                    // Calificación IMDb (imdbRating)
                    if (!string.IsNullOrEmpty(peliculaDTO.imdbRating) && float.TryParse(peliculaDTO.imdbRating, out float imdbRating) && !peliculaDTO.imdbRating.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        float.TryParse(peliculaDTO.imdbRating, out float imdbRatingEnter);
                        pelicula.Schema_aggregateRating = imdbRatingEnter;
                    }

                    //Clasificación de contenido (Rated)
                    if (!string.IsNullOrEmpty(peliculaDTO.Rated) && !peliculaDTO.Rated.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_contentRating = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, List<string>>()
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, new List<string>(){ peliculaDTO.Rated } }
                    };
                    }

                    //url- Website
                    if (!string.IsNullOrEmpty(peliculaDTO.Website) && !peliculaDTO.Website.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        pelicula.Schema_url = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, List<string>>()
                    {
                        { GnossBase.GnossOCBase.LanguageEnum.es, new List<string> { peliculaDTO.Website } }
                    };
                    }

                    // Fecha de lanzamiento
                    if (!string.IsNullOrEmpty(peliculaDTO.Released) && DateTime.TryParse(peliculaDTO.Released, out DateTime releaseDate) && !peliculaDTO.Released.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        DateTime.TryParse(peliculaDTO.Released, out DateTime releaseDateEnter);
                        pelicula.Schema_datePublished = releaseDateEnter;
                    }


                    // Calificación             

                    pelicula.Schema_rating = new();

                    //if (peliculaDTO?.Source.Count() > 0)
                    if (peliculaDTO.Ratings != null && peliculaDTO.Ratings.Count() > 0)
                    {
                        foreach (var ratingDTO in peliculaDTO.Ratings)
                        {
                            Rating rating = new Rating();
                            rating.Schema_ratingSource = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>();
                            rating.Schema_ratingSource.Add(GnossBase.GnossOCBase.LanguageEnum.es, ratingDTO.Source);
                            rating.Schema_ratingValue = ratingDTO.Value;
                            pelicula.Schema_rating.Add(rating);
                        }
                    }


                    //Generos

                    pelicula.IdsSchema_genre = new();
                    string[] generos = peliculaDTO.Genre.Split(',');
                    //List
                    foreach (var genero in generos)
                    {
                        string generotrim = genero.Trim();
                        string uriGenero = t_URIsNombresGenerosExistentes.Where(genero => genero.Item2 == generotrim).FirstOrDefault().Item1;

                        if (uriGenero != null)
                        {
                            pelicula.IdsSchema_genre.Add(uriGenero);

                        }
                    };

                    pelicula.IdsSchema_actor = new();
                    string[] actores = peliculaDTO.Actors.Split(',');
                    foreach (var actor in actores)
                    {
                        string actortrim = actor.Trim();
                        string uriActor = t_URIsNombresPersonasExistentes.Where(persona => persona.Item2 == actortrim).FirstOrDefault()?.Item1;
                        if (uriActor != null)
                        {
                            pelicula.IdsSchema_actor.Add(uriActor);
                        }
                    }


                    pelicula.IdsSchema_director = new();
                    string[] directores = peliculaDTO.Director.Split(',');
                    foreach (var director in directores)
                    {
                        string directortrim = director.Trim();
                        string uriDirector = t_URIsNombresPersonasExistentes.Where(persona => persona.Item2 == directortrim).FirstOrDefault()?.Item1;
                        if (uriDirector != null)
                        {
                            pelicula.IdsSchema_director.Add(uriDirector);
                        }
                    }


                    pelicula.IdsSchema_author = new();
                    string[] guionistas = peliculaDTO.Writer.Split(',');
                    foreach (var guionista in guionistas)
                    {
                        string guionistatrim = guionista.Trim();
                        string uriGuionista = t_URIsNombresPersonasExistentes.Where(persona => persona.Item2 == guionistatrim).FirstOrDefault()?.Item1;
                        if (uriGuionista != null)
                        {
                            pelicula.IdsSchema_author.Add(uriGuionista);
                        }
                    }

                    l_Movie.Add(pelicula);

                    try
                    {
                        ComplexOntologyResource peliculaCOR = pelicula.ToGnossApiResource(mResourceApi, null, Guid.NewGuid(), Guid.NewGuid());
                        mResourceApi.LoadComplexSemanticResource(peliculaCOR);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"La película {pelicula.Schema_name} no se ha podido cargar");
                    }

                }
                return l_Movie;
            }


            internal List<Tuple<string, string>> ObtenerRecursos(ResourceApi mResourceApi, string pOntology)
            {
                List<Tuple<string, string>> recursosEnGrafo = new List<Tuple<string, string>>();
                //string[] ontologias = { "personakarmele", "generokarmele" };

                // Construcción de la consulta SPARQL
                string select = $@"SELECT DISTINCT ?s ?nombre";
                string where = $@" WHERE {{                      
                    ?s <http://schema.org/name> ?nombre.
                }}";

                // Ejecutar la consulta SPARQL en la ontología actual
                SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);

                // Procesar los resultados
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (var item in resultadoQuery.results.bindings)
                    {
                        if (item.ContainsKey("nombre"))
                        {
                            string nombre = item["nombre"].value;
                            string uri = item["s"].value;
                            recursosEnGrafo.Add(new Tuple<string, string>(uri, nombre));  // Añadir la URI del recurso a la lista
                        }
                    }
                }


                return recursosEnGrafo;
            }

        internal List<Tuple<string, Awards>> ObtenerPremios(ResourceApi mResourceApi, string pOntology)
        {
            List<Tuple<string, Awards>> recursosEnGrafo = new List<Tuple<string, Awards>>();
            //string[] ontologias = { "personakarmele", "generokarmele" };

            // Construcción de la consulta SPARQL
            string select = $@"SELECT DISTINCT ?s ?nombre";
            string where = $@" WHERE {{  
                    ?s <http://schema.org/category> ?categoria.
                    ?s <http://schema.org/name> ?nombre.
                }}";

            // Ejecutar la consulta SPARQL en la ontología actual
            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);

            // Procesar los resultados
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (var item in resultadoQuery.results.bindings)
                {
                    Awards premioKG = new();
                    if (item.ContainsKey("nombre"))
                    {
                        string uri = item["s"].value;
                        premioKG.Schema_name = item["nombre"].value;
                        
                        recursosEnGrafo.Add(new Tuple<string, Awards>(uri, premioKG));  // Añadir la URI del recurso a la lista
                    }
                }
            }


            return recursosEnGrafo;
        }
    }
    }

