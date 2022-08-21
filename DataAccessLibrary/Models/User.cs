using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(250)]
        public string UserName { get; set; } = string.Empty;


        [Required]
        [MaxLength(250)]
        public string Email { get; set; } = string.Empty;


        [Required]
        [MaxLength(250)]
        public string PasswordHash { get; set; } = string.Empty;


        [Required]
        [MaxLength(250)]
        public string Role { get; set; } = string.Empty;


        [Required]
        public DateTime LastSeen { get; set; }


        [Required]
        public DateTime Created { get; set; }
    }
}
