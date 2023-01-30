using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Data.AuthModels;

public class Client
{
    public int Id { get; set; }

    [MaxLength(40)]
    public string Username { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? Password { get; set; } = null!;

    public DateTime RegDate { get; set; } 
}