using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Application.TypeInputs
{
    public class ProductTypeInput
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
}
