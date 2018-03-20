namespace Miniblog.Core
{
    public class BlogSettings
    {
        public string Owner { get; set; } = "Yankov Viacheslav";
        public int PostsPerPage { get; set; } = 2;
        public int CommentsCloseAfterDays { get; set; } = 10;
    }
}
