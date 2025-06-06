﻿using System.ComponentModel.DataAnnotations;

namespace Cars.DTO
{
    public class UserLoginDTO
    {

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
