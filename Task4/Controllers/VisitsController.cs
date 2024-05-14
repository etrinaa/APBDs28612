using Microsoft.AspNetCore.Mvc;
using Task4.models;

namespace Task4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VisitsController : ControllerBase
    {
        private static List<Visit> visits = new List<Visit>();

        // Retrieve all visits
        [HttpGet]
        public IActionResult GetVisits() => Ok(visits);

        // Retrieve visits for a specific animal by the animal's Id
        [HttpGet("{animalId}")]
        public IActionResult GetVisitsByAnimal(Guid animalId)
        {
            var filteredVisits = visits.Where(v => v.AnimalId == animalId).ToList();
            if (!filteredVisits.Any()) return NotFound("No visits found for this animal.");
            return Ok(filteredVisits);
        }

        // Add a new visit
        [HttpPost]
        public IActionResult AddVisit([FromBody] Visit newVisit)
        {
            visits.Add(newVisit);
            return CreatedAtAction(nameof(GetVisitsByAnimal), new { animalId = newVisit.AnimalId }, newVisit);
        }
    }
}
