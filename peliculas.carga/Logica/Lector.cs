using Newtonsoft.Json;
using peliculas.carga.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peliculas.carga.Logica
{
    internal class Lector
    {
        // Ruta de la carpeta que contiene los archivos JSON
        string _folderPath = @"C:\Users\karmelealonso\Desktop\peliculas.carga\peliculas.carga\DATA\";

        public Lector() { }

        public List<PeliculaDTO> LeerPeliculas(int numPeliculasCargar)
        {
            // Obtener todos los archivos JSON de la carpeta
            string[] jsonFiles = Directory.GetFiles(_folderPath, "*.json");

            // Verificar si realmente se están obteniendo archivos JSON
            Console.WriteLine($"Archivos JSON encontrados: {jsonFiles.Length}");

            // Lista para almacenar todas las películas
            List<PeliculaDTO> l_PeliculaDTO = new List<PeliculaDTO>();

            int contador = 0;

            // Leer cada archivo JSON y deserializarlo
            foreach (var file in jsonFiles)
            {
                try
                {
                    // Leer el archivo JSON
                    string jsonData = File.ReadAllText(file);

                    // Deserializar el JSON a un objeto PeliculaDTO
                    PeliculaDTO pelicula = JsonConvert.DeserializeObject<PeliculaDTO>(jsonData);

                    // Agregar el objeto PeliculaDTO a la lista
                    l_PeliculaDTO.Add(pelicula);

                    // Imprimir el título y el año de la película procesada
                    Console.WriteLine($"Procesando película: {pelicula.Title} ({pelicula.Year})");

                    contador++;

                    if (contador == numPeliculasCargar)
                    {
                        //return l_PeliculaDTO;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, mostrar el archivo y el error
                    Console.WriteLine($"Error al procesar el archivo {file}: {ex.Message}");
                }
            }

            // Mostrar cuántas películas fueron procesadas
            Console.WriteLine($"Películas procesadas: {l_PeliculaDTO.Count}");

            return l_PeliculaDTO;
        }


        // Método auxiliar para convertir una cadena separada por comas en una lista
        internal List<string> ConvertToList(string commaSeparatedValues)
        {
            if (string.IsNullOrEmpty(commaSeparatedValues))
            {
                return new List<string>();
            }
            return new List<string>(commaSeparatedValues.Split(',').Select(value => value.Trim()));
        }
    }
}

