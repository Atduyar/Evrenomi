using System;

namespace Entities.Dtos
{
    public class ErrorResponseDto
    {
        public string Operation { get; set; }
        public string ErrorMessages { get; set; }
    }
}