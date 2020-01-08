using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Models
{
    public class MovieDirector
    {
        public int MovieId { get; set; }
        public int DirectorId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Director Director { get; set; }

    }
}
