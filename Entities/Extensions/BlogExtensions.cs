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
        public static BlogDetailDto ToDetail(this Blog blog, User user, List<Tag> tags)
        {
            //Foo json = JsonConvert.DeserializeObject<Foo>(str);
            return new BlogDetailDto
            {
                BlogId = blog.Id,
                AuthorSummary = user.ToSummary(),
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogDate = blog.BlogDate,
                BlogTags = tags,
                BlogContent = JsonConvert.DeserializeObject<List<BlogElements>>(blog.BlogContent)//blog.BlogContent
                //BlogContent = blog.BlogContent
            };
        }
        public static BlogDetailDto ToDetail(this Blog blog, IDataResult<User> user, IDataResult<List<Tag>> tags)
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

            if (user.Success)
            {
                return new BlogDetailDto
                {
                    BlogId = blog.Id,
                    AuthorSummary = user.Data.ToSummary(),
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

        public static BlogSummaryDto ToSummary(this Blog blog, string authorName, IDataResult<List<Tag>> tags)
        {
            return new BlogSummaryDto
            {
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogId = blog.Id,
                BlogTags = tags.Data,
                Readed = false,
                Views = -1,
                AuthorName = authorName
            };
        }
        public static BlogSummaryDto ToSummary(this Blog blog, string authorName, List<Tag> tags)
        {
            return new BlogSummaryDto
            {
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogId = blog.Id,
                BlogTags = tags,
                Readed = false,
                Views = -1,
                AuthorName = authorName
            };
        }
        public static BlogSummaryDto ToSummary(this Blog blog, string authorName)
        {
            return new BlogSummaryDto
            {
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogId = blog.Id,
                Readed = false,
                Views = -1,
                AuthorName = authorName
            };
        }
        public static BlogSummaryDto ToSummary(this Blog blog)
        {
            return new BlogSummaryDto
            {
                BlogTitle = blog.BlogTitle,
                BlogSummary = blog.BlogSummary,
                BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
                BlogId = blog.Id,
                Readed = false,
                Views = -1
            };
        }
    }
}