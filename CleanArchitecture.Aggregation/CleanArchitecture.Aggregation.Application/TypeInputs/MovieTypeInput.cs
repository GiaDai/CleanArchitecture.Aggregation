using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Application.TypeInputs
{
    public class MovieTypeInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public DateTime? ReleaseDate { get; set; } = DateTime.Now;
    }
}
