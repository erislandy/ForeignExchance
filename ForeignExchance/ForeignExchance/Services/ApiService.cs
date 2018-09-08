

namespace ForeignExchance.Services
{
    using ForeignExchance.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                
                 /* No tiene implementada la api en internet
                var client = new HttpClient();
                client.BaseAddress = new
                    Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucces = false,
                        Message = result
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                */
                var list = RateService.GetRates();
                return new Response
                {
                    IsSucces = true,
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new Response()
                {
                    IsSucces = false,
                    Message = ex.Message
                };
            }
        }
    }
}
