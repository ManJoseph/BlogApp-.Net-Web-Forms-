namespace BlogApp.Models
{
    public class Blog
    {
        public int blogId { get; set; }
        public string title { get; set; } = "";
        public string blogPost { get; set; } = "";
        public string category { get; set; } = "";
        public string blogger { get; set; } = "";
        public string bloggerId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
