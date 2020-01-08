using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+", ErrorMessage="Country Name can only be in Characters")]
        public string Name { get; set; }
        public virtual ICollection<Director> Directors { get; set; }
    }
}
