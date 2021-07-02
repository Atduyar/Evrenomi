using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Entities.Concrete;
using LinqKit;

namespace Business.Extensions
{
    public static class LinqExtensions
    {
        public static Expression<Func<Blog, bool>> ContainsInDescription(
            params string[] keywords)
        {
            var predicate = PredicateBuilder.False<Blog>();
            foreach (string keyword in keywords)
                predicate = predicate.And(b => b.BlogTitle.Contains(keyword));

            return predicate;
        }
    }

}