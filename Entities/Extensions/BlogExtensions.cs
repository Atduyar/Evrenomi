using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Newtonsoft.Json;

namespace Entities.Extensions
{
    public static class BlogExtensions
    {
        public static BlogDetailDto ToDetail(this Blog blog, AuthorSummaryDto authorSummary, List<Tag> tags)
        {
            //Foo json = JsonConvert.DeserializeObject<Foo>(str);
            return new BlogDetailDto
            {
                BlogId = blog.Id,
                AuthorSummary = authorSummary,
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogDate = blog.BlogDate,
                BlogTags = tags,
                BlogContent = JsonConvert.DeserializeObject<List<BlogElements>>(blog.BlogContent)//blog.BlogContent
                //BlogContent = blog.BlogContent
            };
        }
        public static BlogDetailDto ToDetail(this Blog blog, IDataResult<Author> authorSummary, IDataResult<List<Tag>> tags)
        {
            //Foo json = JsonConvert.DeserializeObject<Foo>(str);
            List<Tag> Btags;
            if (tags.Success)
            {
                Btags = tags.Data;
            }
            else
            {
                Btags = new List<Tag>();
            }

            if (authorSummary.Success)
            {
                return new BlogDetailDto
                {
                    BlogId = blog.Id,
                    AuthorSummary = authorSummary.Data.ToSummary(),
                    BlogTitle = blog.BlogTitle,
                    BlogSummary = blog.BlogSummary,
                    BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                    BlogDate = blog.BlogDate,
                    BlogTags = Btags,
                    BlogContent = JsonConvert.DeserializeObject<List<BlogElements>>(blog.BlogContent)//blog.BlogContent
                    //BlogContent = blog.BlogContent
                };
            }
            else
            {
                return new BlogDetailDto
                {
                    BlogId = blog.Id,
                    AuthorSummary = null,
                    BlogTitle = blog.BlogTitle,
                    BlogSummary = blog.BlogSummary,
                    BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                    BlogDate = blog.BlogDate,
                    BlogTags = Btags,
                    BlogContent = JsonConvert.DeserializeObject<List<BlogElements>>(blog.BlogContent)//blog.BlogContent
                    //BlogContent = blog.BlogContent
                };
            }
        }

        //public static BlogViewDto ToViewDetail(this Blog blog)
        //{
        //    return new BlogViewDto
        //    {
        //        BlogId = blog.Id,
        //        AuthorId = blog.AuthorId,
        //        BlogTitle = blog.BlogTitle,
        //        BlogSummary = blog.BlogSummary,
        //        BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
        //        BlogDateT = blog.BlogDate,
        //        BlogTags = blog.BlogContent,
        //        BlogContent = blog.BlogContent
        //    };
        //}

        public static BlogSummaryDto ToSummary(this Blog blog)
        {
            return new BlogSummaryDto
            {
            };
        }
        public static AuthorSummaryDto ToSummary(this Author author)
        {
            //Foo json = JsonConvert.DeserializeObject<Foo>(str);
            return new AuthorSummaryDto()
            {
                UserId = author.UserId,
                AuthorAvatarUrl = author.AuthorAvatarUrl,
                AuthorId = author.Id,
                AuthorName = author.AuthorName,
                AuthorDescription = author.AuthorDescription
            };
        }
    }
}