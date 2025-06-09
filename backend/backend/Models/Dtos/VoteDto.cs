namespace backend.Models.Dtos
{
    public class VoteDto
    {
        public int UserId { get; set; }
        public int PollId { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
    }
}
