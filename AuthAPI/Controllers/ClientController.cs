using Microsoft.AspNetCore.Mvc;
using AuthAPI.Services;
using AuthAPI.Data.AuthModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService _service;
    private IConfiguration config;

    public ClientController(ClientService service, IConfiguration config)
    {
        _service = service;
        this.config = config;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<Client>> Get()
    {
        return await _service.GetAll();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _service.GetById(id);

        if (client is null)
            return NotFound();
        
        return client;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Client client) 
    {
        var newClient = await _service.Create(client);
        string jwt = Jwt(newClient);
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id}, new { token = jwt, id = newClient.Id });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Client client, int id)
    {
        if (id != client.Id)
            return BadRequest();

        var clientToUpdate = await _service.GetById(id);
        if (clientToUpdate is null) 
            return NotFound();

        await _service.Update(client, id);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) 
    {
        var clientToDelete = await _service.GetById(id);
        if (clientToDelete is null)
            return NotFound();

        await _service.Delete(id);
        return Ok();
    }

    private string Jwt(Client client)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, client.Id.ToString())
        };

        var secret = config.GetSection("JWT:Key").Value;
        SymmetricSecurityKey? key = null;
        if(secret is not null) {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMonths(12), signingCredentials: creds);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
}