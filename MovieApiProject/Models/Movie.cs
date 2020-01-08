using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength=3,ErrorMessage="Isan can be between 3 & 10 characters")]
        public string Isan { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Title can't be more than 100 characters")]
        public string Title { get; set; }
        public DateTime? DateReleased { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<MovieDirector> MovieDirectors { get; set; }
        public virtual ICollection<MovieCategory> MovieCategories { get; set; }
    }
}
