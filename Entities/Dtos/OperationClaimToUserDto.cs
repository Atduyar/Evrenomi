using Core.Entities.Concrete;

namespace Entities.Dtos
{
    public class OperationClaimToUserDto
    {
        public UserForLoginDto UserForLoginDto { get; set; }
        public OperationClaim OperationClaim { get; set; }
    }
}   