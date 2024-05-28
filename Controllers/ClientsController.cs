// Controllers/ClientsController.cs
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
    public class ClientsController : ControllerBase
    {
        private readonly maksousDbContext _context;

        public ClientsController(maksousDbContext context)
        {
            _context = context;
        }

        //2

        // DELETE: api/clients/{idClient}
        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
            {
                return NotFound();
            }

            if (client.ClientTrips.Any())
            {
                return BadRequest("The client has assigned trips and cannot be deleted.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //3
        // POST: api/trips/{idTrip}/clients
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientDto clientDto)
        {
            // Check if client with PESEL already exists
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);

            if (existingClient != null)
            {
                return BadRequest("Client with this PESEL number already exists.");
            }

            //check if the trip exists; the DateFrom is in the future
            var trip = await _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip && t.Name == clientDto.TripName);

            if (trip == null || trip.DateFrom < DateTime.Now)
            {
                return BadRequest("Trip does not exist or has already occurred.");
            }

            // check if the client is registered for the trip
            if (trip.ClientTrips.Any(ct => ct.IdClientNavigation.Pesel == clientDto.Pesel))
            {
                return BadRequest("Client is already registered for this trip.");
            }

           
            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            
            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now, 
                PaymentDate = clientDto.PaymentDate 
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.IdClient }, client);
        }

    }
}