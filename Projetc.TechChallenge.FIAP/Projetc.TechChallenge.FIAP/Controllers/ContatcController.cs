using Microsoft.AspNetCore.Mvc;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Models;

namespace Projetc.TechChallenge.FIAP.Controllers;

[ApiController]
[Route("controller")]
public class ContatcController(IContatctRepository repository) : Controller
{

    private readonly IContatctRepository _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _repository.GetAllContactsAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contact = await _repository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return Ok(contact);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactCreateDto contactDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var contact = new Contact
        {
            Name = contactDto.Name,
            Phone = contactDto.Phone,
            Email = contactDto.Email,
            DDD = contactDto.DDD
        };

        await _repository.AddContactAsync(contact);
        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
    {
        if (id != contact.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }
        await _repository.UpdateContactAsync(contact);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _repository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        await _repository.DeleteContactAsync(id);
        return NoContent();
    }
}
