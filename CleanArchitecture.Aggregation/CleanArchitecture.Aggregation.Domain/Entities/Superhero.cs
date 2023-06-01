using CleanArchitecture.Aggregation.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Aggregation.Domain.Entities
{
    public class Superhero : AuditableBaseEntity
    {
        [Required(ErrorMessage = "Please specify a name for the superhero")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Height { get; set; }
        public ICollection<Superpower> Superpowers { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
