using Covauto.Shared.DTOs;
using Covauto.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoController : ControllerBase
    {
        private readonly IAutoRepository _repo;

        public AutoController(IAutoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var autos = await _repo.GetAllAsync();
            return Ok(autos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auto = await _repo.GetByIdAsync(id);
            if (auto == null) return NotFound();

            return Ok(auto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AutoDTO autoDto)
        {
            var createdAuto = await _repo.AddAsync(autoDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAuto.Id }, createdAuto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AutoDTO autoDto)
        {
            var success = await _repo.UpdateAsync(id, autoDto);
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