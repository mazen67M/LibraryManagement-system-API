﻿using System.ComponentModel.DataAnnotations;

public class UserLoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
