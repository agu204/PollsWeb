using backend.Models.Database.Entities;
using backend.Models.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Models.Database.Repositories.Implementations;

public class PollRepository : Repository<Poll, int>
{
    public PollRepository(PollsContext context) : base(context) { }

    public async Task<Poll?> GetPollByIdAsync(int id)
    {
        return await GetQueryable()
            .Include(p => p.Options)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Poll>> GetAllPollsAsync()
    {
        return await GetQueryable()
            .Include(p => p.Options)
            .ToListAsync();
    }

    public async Task<Poll> InsertPollAsync(Poll newPoll)
    {
        await base.InsertAsync(newPoll);
        return newPoll;
    }

    public void Update(Poll poll)
    {
        base.Update(poll);
    }

    public void Delete(Poll poll)
    {
        base.Delete(poll);
    }
}
