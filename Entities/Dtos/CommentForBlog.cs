using System;

namespace Entities.Dtos
{
    public class CommentForBlog
    {
        public int CommentId { get; set; }
        public int CommentResponse { get; set; } = 0;
        public int BlogId { get; set; }/// <summary>
        /// ////////////////////////////////////////////////////////
        /// </summary>
        //public int UserId { get; set; }
        //public string UserAvatarUrl { get; set; }
        //public string UserNickname { get; set; }
        public UserSummaryDto UserSummary { get; set; }
        public string CommentDate { get; set; }
        public string Text { get; set; }
    }
}