using AutoMapper;
using Covauto.Domain.Entities;
using Covauto.Application.Interfaces;
using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeenAutoReserveringController : ControllerBase
    {
        private readonly ILeenAutoReserveringRepository _repo;
        private readonly IMapper _mapper;

        public LeenAutoReserveringController(ILeenAutoReserveringRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reserveringen = await _repo.GetAllAsync();
            var reserveringDtos = _mapper.Map<IEnumerable<LeenAutoReserveringDTO>>(reserveringen);
            return Ok(reserveringDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservering = await _repo.GetByIdAsync(id);
            if (reservering == null) return NotFound();
            var reserveringDto = _mapper.Map<LeenAutoReserveringDTO>(reservering);
            return Ok(reserveringDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeenAutoReserveringDTO reserveringDto)
        {
            var reservering = _mapper.Map<LeenAutoReservering>(reserveringDto);
            var createdReservering = await _repo.AddAsync(reservering);
            var createdReserveringDto = _mapper.Map<LeenAutoReserveringDTO>(createdReservering);
            return CreatedAtAction(nameof(GetById), new { id = createdReserveringDto.Id }, createdReserveringDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeenAutoReserveringDTO reserveringDto)
        {
            var reservering = _mapper.Map<LeenAutoReservering>(reserveringDto);
            var success = await _repo.UpdateAsync(id, reservering);
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