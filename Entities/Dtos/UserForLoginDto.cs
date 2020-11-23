using System.ComponentModel.DataAnnotations;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class UserForLoginDto:IDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(45)]
        [MinLength(3)]
        public string EmailOrNickname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}