using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Sales.Common.Models;
    

    public class ApiService
    {
        public async Task<Response> Getlist<T>(string urlBase, string prefix,string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSucess = false,
                        Message = answer,
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    isSucess = true,
                    Result = list,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    isSucess=false,
                    Message= ex.Message,

                };
            }
        }

    }
}
