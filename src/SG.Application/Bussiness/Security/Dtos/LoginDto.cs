using System.ComponentModel.DataAnnotations;
using SG.Application.CustomValidations;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Auth.JwtAuthentication;

namespace SG.Application.Bussiness.Security.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Please enter your username.")]
    [MinLength(2)] 
    public required string UserName { get; set; }

    [PasswordValidationRegexAttribute(ErrorMessage = "The password must be at least 8 characters long, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 character.")]
    [StringLength(100, ErrorMessage = "Field {0} should be between {2} and {1} characters long.", MinimumLength = 8)]
    public required string Password { get; set; }

    public bool? EvaluateEmail { get; set; }

    public void Deconstruct(out string userName, out string password)
    {
        userName = UserName.Trim();
        password = Password.Trim();
    }
}


public class LoginResponseDto
{
    public int IdUser { get; set; }
    public string DisplayName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public TokenResponse Tokens { get; set; } = default!;

    public LoginResponseDto(User user,string accessToken,string refreshToken)
    {
        IdUser = user.Id;
        DisplayName = user.Username;
        UserName = user.Username;
        Email = user.Email;
        Tokens = new TokenResponse 
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}