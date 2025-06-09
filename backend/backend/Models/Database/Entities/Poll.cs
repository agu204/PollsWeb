namespace backend.Models.Database.Entities
{
    public class Poll
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool AllowsMultipleAnswers { get; set; }

        public bool IsClosed { get; set; } = false;

        public int CreatedByUserId { get; set; }


        // Relaciones
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    }

}
