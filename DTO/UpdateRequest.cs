﻿using System.ComponentModel.DataAnnotations;

namespace DenaAPI.DTO
{
    public class UpdateRequest
    {
        public int Id { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
