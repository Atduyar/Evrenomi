using System.ComponentModel.DataAnnotations;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class UserForRegisterDto:IDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(45)]
        [MinLength(8)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(45)]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        [MinLength(3)]
        public string Nickname { get; set; }
    }
}
