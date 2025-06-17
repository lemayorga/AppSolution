// using System;
// using System.ComponentModel.DataAnnotations;

// namespace SG.Application.Bussiness.Security.Dtos;

// public class RoleDto 
// {
//     public int Id { get; set; }
//     public string CodeRol { get; set; } = string.Empty;
//     public string Name { get; set; }  = string.Empty;
//     public bool IsActive { get; set; }
// }


// public class RoleCreateDto 
// {
//     [Required]
//     public required string CodeRol { get; set; } = string.Empty;
//     [Required]
//     public required string Name { get; set; }  = string.Empty;
// }

// public class RoleUpdateDto  : RoleCreateDto
// {
//     [Required]
//     public bool IsActive { get; set; }
// }