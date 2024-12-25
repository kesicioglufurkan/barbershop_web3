using barbershop_web3.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace barbershop_web3.Controllers
{
    public class SaloonConsumeApiController : Controller
    {

            SaloonContext s = new SaloonContext();
            public async Task<IActionResult> Index()
            {
                List<Saloon> saloons = new List<Saloon>();
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:7030/api/SaloonApi/");
                var jsondata = await response.Content.ReadAsStringAsync();
                saloons = JsonConvert.DeserializeObject<List<Saloon>>(jsondata);
                return View(saloons);
            }

        }

    
}
