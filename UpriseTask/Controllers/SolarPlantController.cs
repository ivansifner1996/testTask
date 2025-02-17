using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpriseTask.Data;
using UpriseTask.Entities;
using UpriseTask.Mappings;
using UpriseTask.Mappings.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpriseTask.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SolarPlantController : ControllerBase
    {
        private readonly SolarPlantContext _context;

        public SolarPlantController(SolarPlantContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolarPlantDto>>> GetSolarPlants()
        {
            var plants = await _context.SolarPlants.Include(p => p.SolarPlantProductions)
                .AsNoTracking().ToListAsync();
            return Ok(plants.Select(plant => plant.MapEntityToBusinessObject()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolarPlantDto>> GetSolarPlant(int id)
        {
            var plant = await _context.SolarPlants.Include(p => p.SolarPlantProductions)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (plant == null) return NotFound();

            return Ok(plant.MapEntityToBusinessObject());
        }

        [HttpPost]
        public async Task<ActionResult<SolarPlantDto>> CreateSolarPlant(SolarPlantDto solarPlantDto)
        {
            var plant = solarPlantDto.MapBusinessObjectToEntity();
            _context.SolarPlants.Add(plant);

            await _context.SaveChangesAsync();

            return Ok(plant.MapEntityToBusinessObject());
        }


        [HttpPost("bulk")]
        public async Task<IActionResult> CreateSolarPlantsBulk(List<SolarPlantDto> solarPlantsDto)
        {
            var plantEntities = solarPlantsDto.Select(plantDto =>
            {
                var plantEntity = plantDto.MapBusinessObjectToEntity(); 

                if (plantDto.SolarPlantProductions != null)
                {
                    plantEntity.SolarPlantProductions = plantDto.SolarPlantProductions
                        .Select(prodDto =>
                        {
                            var productionEntity = prodDto.MapBusinessObjectToEntity();
                            productionEntity.SolarPlant = plantEntity; 
                    return productionEntity;
                        })
                        .ToList();
                }

                return plantEntity;
            }).ToList();

            await _context.SolarPlants.AddRangeAsync(plantEntities);
            await _context.SaveChangesAsync();

            var plantDtos = plantEntities.Select(p => p.MapEntityToBusinessObject()).ToList();

            return Ok(plantDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSolarPlant(int id, SolarPlantDto solarPlantDto)
        {
            var existingPlant = await _context.SolarPlants
                .Include(p => p.SolarPlantProductions)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existingPlant == null) return NotFound();

            solarPlantDto.MapBusinessObjectToEntity(existingPlant);

            _context.Entry(existingPlant).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(existingPlant.MapEntityToBusinessObject());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolarPlant(int id)
        {

            var plant = await _context.SolarPlants
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant == null) return NotFound();

            _context.SolarPlants.Remove(plant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
