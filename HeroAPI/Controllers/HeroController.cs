using HeroAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {

        private static Dictionary<int, Hero> heros = new Dictionary<int, Hero>()
            {
                {1,new Hero(){ Id=1,Name="Superman",FirstName="Clark",LastName="Kent",Location="SmallVille"} }
            };
        public HeroController()
        {
            
        }

        // GET: api/v1/Hero
        [HttpGet]
        public async Task<ActionResult<IDictionary<int, Hero>>> GetHeros()
        {
            return Ok(heros);
        }

        // GET: api/v1/Hero/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(int id)
        {
            Hero? hero = null;
            heros.TryGetValue(id, out hero);

            if (hero == null)
            {
                return NotFound();
            }
            return Ok(hero);
        }

        // POST: api/v1/Hero
        [HttpPost]
        public async Task<ActionResult<Hero>> AddHero(Hero newHero)
        {
            heros.Add(newHero.Id,newHero);
            return CreatedAtAction("GetHeroItem", new { id = newHero.Id }, newHero);
        }

        // POST: api/v1/Hero
        [HttpPut]
        public async Task<ActionResult<Hero>> UpdateHero(int id,Hero updatedHero)
        {
            if (id != updatedHero.Id)
            {
                return BadRequest();
            }
            if(!heros.ContainsKey(id))
            {
                return NotFound();
            }
            heros[id] = updatedHero;

            return NoContent();
        }

        // DELETE: api/v1/Hero
        [HttpDelete]
        public async Task<ActionResult<Hero>> DeleteHero(int id)
        {            
            if (!heros.ContainsKey(id))
            {
                return NotFound();
            }
            heros.Remove(id);

            return NoContent();
        }
    }

}
