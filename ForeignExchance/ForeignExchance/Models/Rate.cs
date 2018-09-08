using System;
using System.Collections.Generic;
using System.Linq;

namespace ForeignExchance.Models
{
    public class Rate
    {

        public int RateId { get; set; }
        public string Code { get; set; }
        public double TaxtRate { get; set; }
        public string Name { get; set; }

    }
}
