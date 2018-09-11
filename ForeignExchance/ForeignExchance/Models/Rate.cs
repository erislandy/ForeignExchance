

namespace ForeignExchance.Models
{
    using SQLite.Net.Attributes;
    public class Rate
    {
        [PrimaryKey]
        public int RateId { get; set; }
        public string Code { get; set; }
        public double TaxtRate { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return RateId;
        }

    }
}
