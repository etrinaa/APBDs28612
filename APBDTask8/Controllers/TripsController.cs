// Controllers/TripsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBDTask8.Context;
using APBDTask8.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APBDTask8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly maksousDbContext _context;

        public TripsController(maksousDbContext context)
        {
            _context = context;
        }
        //1

        // GET: api/trips
        [HttpGet]
        public async Task<ActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var tripsQuery = _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom);

            var totalTrips = await tripsQuery.CountAsync();
            var trips = await tripsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                pageNum = page,
                pageSize = pageSize,
                allPages = (int)Math.Ceiling(totalTrips / (double)pageSize),
                trips
            };

            return Ok(result);
        }
    }
}