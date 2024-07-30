using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Models;
using Projetc.TechChallenge.FIAP.Services;

namespace Projetc.TechChallenge.FIAP.Controllers;

[ApiController]
[Route("/controller")]
public class ContatcController(IContatctRepository repository) : Controller
{
    private readonly IResponseService _responseService;
    private readonly IContatctRepository _repository = repository;

    ResponseService service = new ResponseService();


    [HttpGet("/GetAllContacts")]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _repository.GetAllContactsAsync();

        await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, contacts);
        return new EmptyResult();
    }

    [HttpGet("/GetContactById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contact = await _repository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, id);
        return new EmptyResult();
    }


    [HttpPost("/AddNewContact")]
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
        await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, contactDto); 
        return new EmptyResult();
    }

    [HttpPut("/UpdateContact")]
    public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
    {
        if (id != contact.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }
        await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, contact);
        return new EmptyResult();
    }

    [HttpDelete("/DeleteContact/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _repository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        await _repository.DeleteContactAsync(id);
         await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, id);
        return new EmptyResult();
    }
}
