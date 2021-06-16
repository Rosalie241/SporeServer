/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System.ComponentModel.DataAnnotations;

namespace SporeServer.Models
{
    public class RegisterNewQuery
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
