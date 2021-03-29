using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Models
{
    public class RegisterInfo
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Email is required.")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password should be 5-20 letters or numbers.")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(16, MinimumLength = 5, ErrorMessage = "Screen Name should be 5-16 letters or numbers.")]
        [Required(ErrorMessage = "Screen Name is required.")]
        public string DisplayName { get; set; }
    }
}
