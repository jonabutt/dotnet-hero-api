using HeroAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {             
        private readonly DataContext _context;

        public HeroController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Hero
        [HttpGet]
        public async Task<ActionResult<IDictionary<int, Hero>>> GetHeros()
        {
            return Ok(await _context.Heroes.ToDictionaryAsync(heros=>heros.Id, heros => heros));
        }

        // GET: api/v1/Hero/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(int id)
        {
            Hero? hero = await _context.Heroes.FindAsync(id);

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
            await _context.Heroes.AddAsync(newHero);
            await _context.SaveChangesAsync();
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
            Hero? hero = await _context.Heroes.FindAsync(id);
            if(hero==null)
            {
                return NotFound();
            }
            hero.Name = updatedHero.Name;
            hero.FirstName = updatedHero.FirstName;
            hero.LastName = updatedHero.LastName;
            hero.Location = updatedHero.Location;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/Hero
        [HttpDelete]
        public async Task<ActionResult<Hero>> DeleteHero(int id)
        {
            Hero? hero = _context.Heroes.Find(id);
            if (hero==null)
            {
                return NotFound();
            }
            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
