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
    private readonly IEmailService _emailService;
    private readonly ILogService _logService;

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

        bool contactExists = await _repository.ContactExistsAsync(contactDto.Name, contactDto.Phone, contactDto.Email, contactDto.DDD);
        if (contactExists)
        {
          await _logService.LogAsync(null, "Contact Creation Attempt", $"Failed to create contact with email {contactDto.Email}. Contact already exists.");
            return Conflict("Contact already exists.");
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
        await _emailService.SendEmailAsync(EmailMessageType.ContactCreated, contact);
        await _logService.LogAsync(contact.Id, "Contact Created", $"A new contact named {contact.Name} was created.");
        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
    }

    [HttpPut("/UpdateContact")]
    public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
    {
        if (id != contact.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }
        await service.WriteResponseAsync(HttpContext, StatusCodes.Status200OK, contact);
       await _emailService.SendEmailAsync(EmailMessageType.ContactUpdated, contact);
        await _logService.LogAsync(contact.Id, "Contact update", $"A contact named {contact.Name} was update.");
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
        await _emailService.SendEmailAsync(EmailMessageType.ContactDeleted, contact);
        await _logService.LogAsync(contact.Id, "Contact deleted", $"A contact named {contact.Name} was deleted.");
        return new EmptyResult();
    }
}
