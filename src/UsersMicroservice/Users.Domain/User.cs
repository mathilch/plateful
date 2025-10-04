namespace Users.Domain;

public class User
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } //hashed?
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}