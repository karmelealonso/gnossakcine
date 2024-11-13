using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peliculas.carga.DTO
{
    internal class PersonaDTOWikidata
    {

        // Nombre de la persona
        public string Nombre { get; set; }

        // URL de la imagen de la persona
        public string Image { get; set; }

        // Sexo o género 
        public string SexOrGender { get; set; }

        // País de ciudadanía
        public string CountryOfCitizenship { get; set; }

        // Fecha de nacimiento
        public DateTime? DateOfBirth { get; set; }

        // Lugar de nacimiento
        public string PlaceOfBirth { get; set; }

        // Ocupación 
        public List<string> Occupation { get; set; }

        // Obra notable 
        public List<string> NotableWork { get; set; }

        // Premios recibidos
        public List<string> AwardReceived { get; set; }
    }
}
