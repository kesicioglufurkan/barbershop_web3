using barbershop_web3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace barbershop_web3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaloonApiController : ControllerBase
    {
        SaloonContext s = new SaloonContext();
        // GET: api/<SaloonApiController>
        [HttpGet]
        public List<Saloon> Get()
        {
            var saloons = s.Saloons.ToList();

            //return yazarlar;
            return saloons;
        }


        // POST api/<SaloonApiController>
        [HttpPost]
        public ActionResult Post([FromBody] Saloon sa)
        {
            s.Saloons.Add(sa);
            s.SaveChanges();
            return Ok(sa.SaloonName + " adlı salon eklendi");

        }


    }
}
