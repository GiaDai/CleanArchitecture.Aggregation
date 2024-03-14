using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Domain.Entities
{
    public class UserPoker
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int Point { get; set; }

        public bool IsAdmin { get; set; }

        public bool ShowPoint { get; set; }
    }
}
