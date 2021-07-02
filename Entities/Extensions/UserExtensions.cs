using System;
using Core.Entities.Concrete;
using Entities.Dtos;

namespace Entities.Extensions
{
    public static class UserExtensions
    {
        public static UserDetailsDto ToDetail(this User user)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Description = user.Description,
                AvatarUrl = !String.IsNullOrEmpty(user.AvatarUrl) ? user.AvatarUrl : "defaultPp.png"
            };
        }
        public static UserSummaryDto ToSummary(this User user)
        {
            return new UserSummaryDto
            {
                Id = user.Id,
                Nickname = user.Nickname,
                AvatarUrl = user.AvatarUrl
            };
        }
    }
}