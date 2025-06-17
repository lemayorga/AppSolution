// using System.ComponentModel.DataAnnotations;
// using SG.Application.CustomValidations;

// namespace SG.Application.Bussiness.Security.Dtos;

// // public class UserDto : BaseDto
// // {
// //     public string Username { get; set; } = string.Empty;
// //     public string Email { get; set; } = string.Empty;
// // //     public string Firstname { get; set; } = string.Empty;
// //     public string Lastname { get; set; } = string.Empty;    
// //     public bool IsActive { get; set; }
// //     public bool IsLocked { get; set; }   
// // }


// // public class UserCreateDto 
// // {
// //     [Required(ErrorMessage = "Please enter your username.")]
// //     [MinLength(2)]   
// //     public required string Username { get; set; }

// //     [Required(ErrorMessage = "Ingrese un correo electrónico válido.")]
// //     [EmailAddress]
// //     public required string Email { get; set; }
// //     public string Firstname { get; set; } = string.Empty;
// //     public string Lastname { get; set; } = string.Empty;
    
// //     [PasswordValidationRegexAttribute(ErrorMessage = "The password must be at least 8 characters long, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 character.")]
// //     [StringLength(100, ErrorMessage = "Field {0} should be between {2} and {1} characters long.", MinimumLength = 8)]
// //     public required string Password { get; set; }     
// // }

// // public class UserUpdateDto  
// // {
// //     public int Id { get; set; }

// //     [Required(ErrorMessage = "Please enter your username.")]
// //     [MinLength(2)]       
// //     public required string Username { get; set; }

// //     [Required(ErrorMessage = "Ingrese un correo electrónico válido.")]
// //     [EmailAddress]
// //     public required string Email { get; set; }
// //     public string Firstname { get; set; } = string.Empty;
// //     public string Lastname { get; set; } = string.Empty;    
// //     public bool IsActive { get; set; }
// //     public bool IsLocked { get; set; }
// // }


// // public class UserChangePassword
// // {
// //     [Required(ErrorMessage = "Please enter your username.")]
// //     [MinLength(2)]  
// //     public required string UserName { get; set; }

// //     [PasswordValidationRegexAttribute()]
// //     [StringLength(100, ErrorMessage = "Field {0} should be between {2} and {1} characters long.", MinimumLength = 8)]
// //     public required string CurrentPassword { get; set; } 

// //     [PasswordValidationRegexAttribute()]
// //     [StringLength(100, ErrorMessage = "Field {0} should be between {2} and {1} characters long.", MinimumLength = 8)]
// //     public required string NewPassword { get; set; }

// //     public bool? EvaluateEmail { get; set; }

// //     public void Deconstruct(out string userName, out string currentPassword , out string newPassword)
// //     {
// //         userName = UserName.Trim();
// //         currentPassword = CurrentPassword.Trim();
// //         newPassword = NewPassword.Trim();
// //     }
// // }

// // public class UserResetPassword
// // {
// //     [Required(ErrorMessage = "Please enter your username.")]
// //     [MinLength(2)]  
// //     public required string UserName { get; set; }

// //     public bool? EvaluateEmail { get; set; }
// // }

// // public class UserResetPasswordById
// // {
// //     [Required(ErrorMessage = "Please enter your user id.")]
// //     [MinLength(2)]  
// //     public required string UserId { get; set; }
// // }