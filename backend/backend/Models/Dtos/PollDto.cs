namespace backend.Models.Dtos
{
    public class PollDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool AllowsMultipleAnswers { get; set; }
        public bool IsClosed { get; set; }
        public int CreatedByUserId { get; set; }
        public List<PollOptionDto> Options { get; set; } = new();
    }

    public class PollOptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int VoteCount { get; set; }
    }
}
