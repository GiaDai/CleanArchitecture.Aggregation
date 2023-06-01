using System;
using System.Collections.Generic;

namespace CleanArchitecture.Aggregation.Application.TypeInputs
{
    public class SuperHeroTypeInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Height { get; set; }
        public ICollection<SuperPowerTypeInput> Superpowers { get; set; }

        public ICollection<MovieTypeInput> Movies { get; set; }
    }
}
