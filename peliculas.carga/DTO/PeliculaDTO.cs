using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peliculas.carga.DTO
{
    internal class PeliculaDTO
    {
        public string Title { get; set; }


        public string Writer { get; set; } 
        public string Director { get; set; } 
        public string Actors { get; set; } 


        public string Genre { get; set; }
        public string Production { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public string Runtime { get; set; }
        public string Language { get; set; }
        public string Awards { get; set; }

        public string imdbRating { get; set; }
        public string Rated { get; set; }
        public string Website { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string Released { get; set; }

        // Ratings contiene una lista de objetos con Source y Value
        public List<RatingDTO> Ratings { get; set; }
    }

    
    // Clase adicional para representar los Ratings
    internal class RatingDTO
    {
        public string Source { get; set; }
        public string Value { get; set; }
    }
}
