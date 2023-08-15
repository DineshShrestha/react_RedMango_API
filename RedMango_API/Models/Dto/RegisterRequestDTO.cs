﻿using System.ComponentModel.DataAnnotations;

namespace RedMango_API.Models.Dto
{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}