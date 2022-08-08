namespace WebTest.Models
{
    public class MovieShareInput
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string? Description { get; set; }

        public long UserId { get; set; }
    }

    public class MovieShareModel
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string? Description { get; set; }

        public bool IsPublish { get; set; } = false;

        public List<long>? UserIds { get; set; }
    }
}
