namespace backend.Models.Database.Entities
{
    public class Vote
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PollId { get; set; }

        public ICollection<PollOption> SelectedOptions { get; set; } = new List<PollOption>();

        // Relaciones
        public Poll Poll { get; set; } = null!;
    }
}
