using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Models
{
    public class MovieCategory
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Category Category { get; set; }
    }
}
