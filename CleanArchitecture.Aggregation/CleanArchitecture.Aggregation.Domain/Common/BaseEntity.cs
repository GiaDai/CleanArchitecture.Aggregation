﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
    }
}
