﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email { get; set; }
        
    }
}
