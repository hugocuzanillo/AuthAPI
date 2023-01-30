using Microsoft.EntityFrameworkCore;
using AuthAPI.Data.AuthModels;

namespace AuthAPI.Data;

public class AuthContext : DbContext
{
    public AuthContext (DbContextOptions<AuthContext> options): base(options) 
    {

    }

    public DbSet<Client> Clients => Set<Client>();
}