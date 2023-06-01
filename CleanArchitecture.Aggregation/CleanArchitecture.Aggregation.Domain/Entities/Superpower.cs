using CleanArchitecture.Aggregation.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Aggregation.Domain.Entities
{
    public class Superpower : AuditableBaseEntity
    {
        public string SuperPower { get; set; }
        public string Description { get; set; }

        [ForeignKey("SuperheroId")]
        public int SuperheroId { get; set; }
        public Superhero Superhero { get; set; }
    }
}
