

namespace ForeignExchance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    public static class RateService
    {
        public static List<Rate> GetRates()
        {
            var list =  new List<Rate>
            {
                new Rate()
                {
                    RateId = 15,
                    Code = "BHD",
                    TaxtRate = 0.377029,
                    Name = "Bahraini Dinar"
                },
                new Rate()
                {
                    RateId = 16,
                    Code = "BIF",
                    TaxtRate = 1737.1,
                    Name = "Burundian Franc"
                },
                new Rate()
                {
                    RateId = 17,
                    Code = "BMD",
                    TaxtRate = 1,
                    Name = "Bermudan Dollar"
                },
                new Rate()
                {
                    RateId = 18,
                    Code = "BND",
                    TaxtRate = 1.363651,
                    Name = "Brunei Dollar"
                },

                 new Rate()
                {
                    RateId = 19,
                    Code = "CUC",
                    TaxtRate = 1,
                    Name = "CUC"
                },
                  new Rate()
                {
                    RateId = 20,
                    Code = "CUP",
                    TaxtRate = 25,
                    Name = "CUP"
                }
            };
            return list;
        }

        public static Rate GetRateByName(string name)
        {
            
            foreach (var rate in GetRates())
            {
                if (rate.Name == name)
                    return rate;
            }
            return null;
        }
    }
}
