using Covauto.Shared.DTOs;
using Covauto.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeenAutoReserveringController : ControllerBase
    {
        private readonly ILeenAutoReserveringRepository _repo;

        public LeenAutoReserveringController(ILeenAutoReserveringRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reserveringen = await _repo.GetAllAsync();
            return Ok(reserveringen);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservering = await _repo.GetByIdAsync(id);
            if (reservering == null) return NotFound();

            return Ok(reservering);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeenAutoReserveringDTO reserveringDto)
        {
            var createdReservering = await _repo.AddAsync(reserveringDto);
            return CreatedAtAction(nameof(GetById), new { id = createdReservering.Id }, createdReservering);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeenAutoReserveringDTO reserveringDto)
        {
            var success = await _repo.UpdateAsync(id, reserveringDto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
