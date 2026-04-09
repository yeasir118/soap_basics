using Microsoft.AspNetCore.Mvc;
using SoapWrapper.Application.DTOs.REST;
using SoapWrapper.Application.Services;

namespace SoapWrapper.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser([FromRoute]int id)
    {
        var user = await _service.GetUser(id);

        if (user == null) return NotFound();

        return Ok(new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
        });
    }
}
