using Covauto.Domain;
using Covauto.Application.DTOs;
using Covauto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeenAutoRitController : ControllerBase
{
    private readonly LeenAutoRitRepository _repository;

    public LeenAutoRitController(LeenAutoRitRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ritten = await _repository.GetAllAsync();
        return Ok(ritten);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rit = await _repository.GetByIdAsync(id);
        if (rit == null) return NotFound();
        return Ok(rit);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeenAutoRitDTO dto)
    {
        var rit = new LeenAutoRit
        {
            WerknemerId = dto.WerknemerId,
            VanAdres = dto.VanAdres,
            NaarAdres = dto.NaarAdres,
            VertrekTijd = dto.VertrekTijd,
            KilometerstandBegin = dto.KilometerstandBegin,
            KilometerstandEind = dto.KilometerstandEind
        };

        var added = await _repository.AddAsync(rit);
        return CreatedAtAction(nameof(GetById), new { id = added.Id }, added);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeenAutoRitDTO dto)
    {
        var updatedRit = new LeenAutoRit
        {
            WerknemerId = dto.WerknemerId,
            VanAdres = dto.VanAdres,
            NaarAdres = dto.NaarAdres,
            VertrekTijd = dto.VertrekTijd,
            KilometerstandBegin = dto.KilometerstandBegin,
            KilometerstandEind = dto.KilometerstandEind
        };

        var success = await _repository.UpdateAsync(id, updatedRit);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repository.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}