using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yucom.Entity;

namespace Yucom.Controllers
{
    [ApiController]
    [Route("api/Asiento")]
    public class AsientoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AsientoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Asiento>> Get (int id)
        {
            var idAsiento = await context.Asientos.FirstOrDefaultAsync(a => a.Id == id);
            if (idAsiento == null)
            {
                return BadRequest($"El comediante de ID {id} no se encuentra en los datos");
            }
            return idAsiento;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Asiento>>> get()
        {
            return await context.Asientos.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult> Post (Asiento asiento)
        {
            var ExisteAsiento = await context.Asientos.AnyAsync(x => x.Numero == asiento.Numero);
            if(ExisteAsiento)
            {
                return BadRequest($"El asiento: {asiento.Numero} ya Existe en el sistema");
            }
            context.Add(asiento);
            await context.SaveChangesAsync();
            return Ok();
        } 


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (Asiento asiento, int id)
        {
            if (asiento.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Asientos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Update(asiento);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Asientos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Asiento(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}