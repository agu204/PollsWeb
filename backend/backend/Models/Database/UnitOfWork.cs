using backend.Models.Database;
using backend.Models.Database.Repositories.Implementations;

namespace backend.Models.Database
{
    public class UnitOfWork
    {
        private readonly PollsContext _context;

        public UserRepository UserRepository { get; init; }

        public PollRepository PollRepository { get; init; }


        public UnitOfWork(
            PollsContext context,
            UserRepository userRepository,
            PollRepository pollRepository
           
            )
        {
            _context = context;

            UserRepository = userRepository;
            PollRepository = pollRepository;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
