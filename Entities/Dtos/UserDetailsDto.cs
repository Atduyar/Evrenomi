using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        [MinLength(3)]
        public string Nickname { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(20)]
        [MinLength(0)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(20)]
        [MinLength(0)]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [MaxLength(45)]
        [MinLength(3)]
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(45)]
        public string Description { get; set; }
    }
}