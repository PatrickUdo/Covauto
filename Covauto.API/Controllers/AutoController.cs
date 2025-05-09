using AutoMapper;
using Covauto.Domain.Entities;
using Covauto.Application.Interfaces;
using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoController : ControllerBase
    {
        private readonly IAutoRepository _repo;
        private readonly IMapper _mapper;

        public AutoController(IAutoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var autos = await _repo.GetAllAsync();
            var autoDtos = _mapper.Map<IEnumerable<AutoDTO>>(autos);
            return Ok(autoDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auto = await _repo.GetByIdAsync(id);
            if (auto == null) return NotFound();
            var autoDto = _mapper.Map<AutoDTO>(auto);
            return Ok(autoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AutoDTO autoDto)
        {
            var auto = _mapper.Map<Auto>(autoDto);
            var createdAuto = await _repo.AddAsync(auto);
            var createdAutoDto = _mapper.Map<AutoDTO>(createdAuto);
            return CreatedAtAction(nameof(GetById), new { id = createdAutoDto.Id }, createdAutoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AutoDTO autoDto)
        {
            var auto = _mapper.Map<Auto>(autoDto);
            var success = await _repo.UpdateAsync(id, auto);
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