using System.Collections.Generic;
using Core.Entities.Concrete;
using Entities.Dtos;

namespace Admin.Models.Results
{
    public class UsersWModel
    {
        public List<User> Users { get; set; }
        public string Token { get; set; }
    }
}