using backend.Models.Database.Entities;
using backend.Models.Dtos;

public class PollMapper
{
    public PollDto PollToDto(Poll poll)
    {
        return new PollDto
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description,
            AllowsMultipleAnswers = poll.AllowsMultipleAnswers,
            IsClosed = poll.IsClosed,
            CreatedByUserId = poll.CreatedByUserId, // 🔥 IMPORTANTE
            Options = poll.Options.Select(o => new PollOptionDto
            {
                Id = o.Id,
                Text = o.Text,
                VoteCount = o.VoteCount
            }).ToList()
        };
    }

    public Poll PollCreateDtoToPoll(PollCreateDto dto)
    {
        return new Poll
        {
            Title = dto.Title,
            Description = dto.Description,
            AllowsMultipleAnswers = dto.AllowsMultipleAnswers,
            Options = dto.Options.Select(opt => new PollOption
            {
                Text = opt
            }).ToList()
        };
    }
}
