using AutoMapper;
using Covauto.Application.Interfaces;
using Covauto.Domain.Entities;
using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeenAutoRitController : ControllerBase
{
    private readonly ILeenAutoRitRepository _repository;
    private readonly IMapper _mapper;

    public LeenAutoRitController(ILeenAutoRitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ritten = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<LeenAutoRitDTO>>(ritten);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rit = await _repository.GetByIdAsync(id);
        if (rit == null) return NotFound();
        var result = _mapper.Map<LeenAutoRitDTO>(rit);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeenAutoRitDTO dto)
    {
        var rit = _mapper.Map<LeenAutoRit>(dto);
        var added = await _repository.AddAsync(rit);
        var result = _mapper.Map<LeenAutoRitDTO>(added);
        return CreatedAtAction(nameof(GetById), new { id = added.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeenAutoRitDTO dto)
    {
        var updatedRit = _mapper.Map<LeenAutoRit>(dto);
        var success = await _repository.UpdateAsync(id, updatedRit);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repository.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}