namespace Entities.Dtos
{
    public class AuthorSummaryDto
    {
        public int AuthorId { get; set; }
        public int UserId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public string AuthorDescription { get; set; }
    }
}