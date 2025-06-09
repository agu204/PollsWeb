using backend.Models.Database;
using backend.Models.Database.Entities;
using backend.Models.Dtos;

namespace backend.Services
{
    public class PollService
    {
        private readonly UnitOfWork _unitOfWork;

        public PollService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Crear una nueva encuesta
        public async Task<Poll> CreatePollAsync(PollCreateDto dto, int createdByUserId)
        {
            var poll = new Poll
            {
                Title = dto.Title,
                Description = dto.Description,
                AllowsMultipleAnswers = dto.AllowsMultipleAnswers,
                CreatedByUserId = createdByUserId,
                Options = dto.Options.Select(opt => new PollOption
                {
                    Text = opt,
                    VoteCount = 0
                }).ToList()
            };

            await _unitOfWork.PollRepository.InsertPollAsync(poll);
            await _unitOfWork.SaveAsync();

            return poll;
        }

        // Obtener todas las encuestas
        public async Task<List<Poll>> GetAllPollsAsync()
        {
            return await _unitOfWork.PollRepository.GetAllPollsAsync();
        }

        // Obtener una encuesta por ID
        public async Task<Poll?> GetPollByIdAsync(int id)
        {
            return await _unitOfWork.PollRepository.GetPollByIdAsync(id);
        }

        // Finalizar encuesta (solo autor puede cerrarla)
        public async Task<bool> ClosePollAsync(int pollId, int userId)
        {
            var poll = await _unitOfWork.PollRepository.GetPollByIdAsync(pollId);

            if (poll == null || poll.CreatedByUserId != userId)
                return false;

            poll.IsClosed = true;
            _unitOfWork.PollRepository.Update(poll);
            await _unitOfWork.SaveAsync();

            return true;
        }

        // Eliminar encuesta (solo admin)
        public async Task<bool> DeletePollAsync(int id)
        {

            var poll = await _unitOfWork.PollRepository.GetPollByIdAsync(id);
            if (poll == null)
                return false;

            _unitOfWork.PollRepository.Delete(poll);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
