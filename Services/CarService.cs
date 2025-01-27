using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SPJApiPublica.Models;

namespace SPJApiPublica.Services
{
    public class CarService
    {
        private readonly HttpClient _httpClient;

        public CarService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", "XYyK8LuhRBrMCAqzIp+yag==38uMSAcz4WMaJb49");
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            var response = await _httpClient.GetAsync("https://api.api-ninjas.com/v1/cars");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Car>>();
            }
            return new List<Car>();
        }
    }
}
