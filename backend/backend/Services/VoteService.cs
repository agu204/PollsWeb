using backend.Models.Database;
using backend.Models.Database.Entities;
using backend.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class VoteService
    {
        private readonly PollsContext _context;

        public VoteService(PollsContext context)
        {
            _context = context;
        }

        public async Task<string> VoteAsync(VoteDto voteDto)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == voteDto.PollId);

            if (poll == null)
                return "La encuesta no existe.";

            if (poll.IsClosed)
                return "La encuesta está cerrada.";

            // Validar opciones seleccionadas
            var selectedOptions = poll.Options
                .Where(o => voteDto.SelectedOptionIds.Contains(o.Id))
                .ToList();

            if (!poll.AllowsMultipleAnswers && selectedOptions.Count > 1)
                return "Esta encuesta solo permite una respuesta.";

            var alreadyVoted = await _context.Votes
                .AnyAsync(v => v.UserId == voteDto.UserId && v.PollId == voteDto.PollId);

            if (alreadyVoted)
            {
                var existingVote = await _context.Votes
                    .Include(v => v.SelectedOptions)
                    .FirstAsync(v => v.UserId == voteDto.UserId && v.PollId == voteDto.PollId);

                // Restar votos antiguos
                foreach (var oldOption in existingVote.SelectedOptions)
                {
                    oldOption.VoteCount--;
                }

                existingVote.SelectedOptions.Clear();

                foreach (var option in selectedOptions)
                {
                    option.VoteCount++; 
                    existingVote.SelectedOptions.Add(option);
                }

                await _context.SaveChangesAsync();
                return "Voto actualizado correctamente.";
            }

            // Nuevo voto
            foreach (var option in selectedOptions)
            {
                option.VoteCount++;
            }

            var vote = new Vote
            {
                PollId = poll.Id,
                UserId = voteDto.UserId,
                SelectedOptions = selectedOptions,
                Poll = poll
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return "Voto registrado correctamente.";
        }

        public async Task<bool> HasUserVotedAsync(int pollId, int userId)
        {
            return await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId);
        }

    }
}
