using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength=1, ErrorMessage="Must be between 1 & 10 characters")]
        public string Headline { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Must be between 10 & 50 characters")]
        public string ReviewMovie { get; set; }
        [Required]
        [Range(1,5, ErrorMessage="Ratings must be between 1 & 5")]
        public int Rating { get; set; }
        public virtual Critic Critic { get; set; }
        public virtual Movie Movie { get; set; }

    }
}
