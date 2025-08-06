// using System.ComponentModel.DataAnnotations;
// using System.Text.RegularExpressions;
// using SG.Shared.Constants;

// namespace SG.Application.CustomValidations;

// public class PasswordValidationRegexsAttribute : ValidationAttribute
// {
//     public PasswordValidationRegexsAttribute()
//     {
//         ErrorMessage = string.Empty;
//     }
//     public override bool IsValid(object? value)
//     {
//         if (value == null) return true;

//         var val = value.ToString() ?? "";
//         if (val.Length < 14) return false;
//         if (!Regex.IsMatch(val, @"[A-Z]")) return false; // Al menos una letra mayúscula
//         if (!Regex.IsMatch(val, @"[a-z]")) return false; // Al menos una letra minúscula
//         if (!Regex.IsMatch(val, @"\d")) return false; // Al menos un número
//         if (!Regex.IsMatch(val, @"[\W_]")) return false; // Al menos un carácter especial
//         return true;
//     }
// }


// public class PasswordValidationRegexAttribute : ValidationAttribute
// {
//     public PasswordValidationRegexAttribute()
//     {
//         ErrorMessage = MESSAGE_CONSTANTS.VALIDATION_PASSWORD_REGEX;
//     }

    
//     public override bool IsValid(object? value)
//     {
//         if (value == null) return true;

//         var val = value.ToString() ?? "";
//         if (!Regex.IsMatch(val, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{8,}$")) return false; 
//         //La contraseña debe tener mínimo 8 caracteres, al menos 1 letra mayúscula, 1 letra minúscula, 1 número y 1 carácter
//         return true;
//     }
// }
