using peliculas.carga.DTO;
using PersonakarmeleOntology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace peliculas.carga.Logica
{
    class Utils
    {
        public Utils() { }

        public static HashSet<string> SepararGeneros(string? generos, HashSet<string> hs_nombreGeneros)
        {
            if (generos != null)
            {
                string[] a_generos = generos.Split(',');
                foreach (var genero in a_generos)
                {

                    if (!genero.Trim().ToLower().Equals(("N/A").ToLower()))
                    {
                        hs_nombreGeneros.Add(genero.Trim());
                    }
                }
            }
            return hs_nombreGeneros;
        }

        public static HashSet<string> SepararPersonas(PeliculaDTO peliculaDTO, HashSet<string> hs_nombrePersonas)
        {
            hs_nombrePersonas = SepararTipoPersonas(peliculaDTO.Director, hs_nombrePersonas);
            hs_nombrePersonas = SepararTipoPersonas(peliculaDTO.Writer, hs_nombrePersonas);
            hs_nombrePersonas = SepararTipoPersonas(peliculaDTO.Actors, hs_nombrePersonas);
            return hs_nombrePersonas;
        }


        public static HashSet<string> SepararPremios(string premios)
        {
            //if (generos != null)
            //{
            //    string[] a_generos = generos.Split(',');
            //    foreach (var genero in a_generos)
            //    {

            //        if (!genero.Trim().ToLower().Equals(("N/A").ToLower()))
            //        {
            //            hs_nombreGeneros.Add(genero.Trim());
            //        }
            //    }
            //}
            //return hs_nombreGeneros;
            return new HashSet<string>();
        }

        public static HashSet<string> SepararTipoPersonas(string personas, HashSet<string> hs_nombrePersonas)
        {
            if (personas != null)
            {
                string[] a_personas = personas.Split(',');
                foreach (var persona in a_personas)
                {
                    // Eliminar paréntesis y el texto dentro de ellos
                    string cleanedPersona = Regex.Replace(persona, @"\s*\(.*?\)", "").Trim();

                    if (!cleanedPersona.ToLower().Equals(("N/A").ToLower()))
                    {
                        hs_nombrePersonas.Add(cleanedPersona);
                    }
                }                
            }
            return hs_nombrePersonas;
        }
    }
}