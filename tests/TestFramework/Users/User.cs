namespace TestFramework.Users;

public class User
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class TokenDto
{
    public string Token { get; set; } = null!;
}