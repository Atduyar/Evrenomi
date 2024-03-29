﻿using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal:EfEntityRepositoryBase<User,VikingContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new VikingContext())
            {
                var result = from operationClaim in context.operationclaims
                    join userOperationClaim in context.useroperationclaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }
        }
    }
}