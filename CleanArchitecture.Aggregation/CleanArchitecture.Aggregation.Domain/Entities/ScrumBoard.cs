using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Domain.Entities
{
    public class ScrumBoard
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserPoker> Users { get; set; } = new List<UserPoker>();
    }
}
