using CleanArchitecture.Aggregation.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Aggregation.Domain.Entities
{
    public class Movie : AuditableBaseEntity
    {
        [Required(ErrorMessage = "The movie title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public DateTime ReleaseDate { get; set; }

        [ForeignKey("SuperheroId")]
        public int SuperheroId { get; set; }
        public Superhero Superhero { get; set; }
    }
}
