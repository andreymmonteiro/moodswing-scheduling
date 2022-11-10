using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using System.Threading.Tasks;

namespace Moodswing.Domain.Services
{
    public interface INotAvailableScheduleUseCase
    {
        Task<BaseResultDto> GetNotAvailableSchedulesAsync(NotAvailableScheduleRequestDto dto);
    }
}
