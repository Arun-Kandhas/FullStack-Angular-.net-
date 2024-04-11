using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.DTO.Contacts
{
    public class CreateContactDTO
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [MaxLength(6)]
        public string PostalCode { get; set; }
    }
}
