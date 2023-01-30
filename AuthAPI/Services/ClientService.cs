using Microsoft.EntityFrameworkCore;
using AuthAPI.Data;
using AuthAPI.Data.AuthModels;

namespace AuthAPI.Services;

public class ClientService
{
    private readonly AuthContext _context;

    public ClientService(AuthContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> Create(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        return client;
    }

    public async Task Update(Client client, int id)
    {
        var clientToUpdate = await _context.Clients.FindAsync(id);
        if (clientToUpdate is not null)
        {
            clientToUpdate.Username = client.Username;
            clientToUpdate.Email = client.Email;
            clientToUpdate.Password = client.Password;

            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var clientToDelete = await _context.Clients.FindAsync(id);
        if (clientToDelete is not null)
        {
            _context.Clients.Remove(clientToDelete);
            await _context.SaveChangesAsync();
        }
    }
}