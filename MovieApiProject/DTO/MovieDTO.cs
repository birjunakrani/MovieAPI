using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Isan { get; set; }
        public string Title { get; set; }
        public DateTime? DateReleased { get; set; }
    }
}
