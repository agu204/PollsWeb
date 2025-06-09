namespace backend.Models.Database.Entities
{
    public class PollOption
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int VoteCount { get; set; } = 0;

        public int PollId { get; set; }
        public Poll Poll { get; set; } = null!;
    }

}
