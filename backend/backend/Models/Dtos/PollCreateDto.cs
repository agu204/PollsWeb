namespace backend.Models.Dtos
{
    public class PollCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool AllowsMultipleAnswers { get; set; }
        public List<string> Options { get; set; } = new();
    }
}
