using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Models;
using Moodswing.Domain.Services;
using Moodswing.Domain.Services.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moodswing.Service.Services
{
    public sealed class NotAvailableScheduleUseCase : INotAvailableScheduleUseCase
    {
        private readonly IDapperService<ScheduleNotAvailableResultDto> _dapeprService;

        public NotAvailableScheduleUseCase(IDapperService<ScheduleNotAvailableResultDto> dapeprService)
            => _dapeprService = dapeprService;

        public async Task<BaseResultDto> GetNotAvailableSchedulesAsync(NotAvailableScheduleRequestDto dto)
        {
            var result = await _dapeprService.GetByParamsAsync(ScheduleConstants.SCHEDULE, GetColunms(), WhereClause(dto), default, default, default, default);
            return new SchedulesNotAvailableResultDto(result);
        }

        private static Dictionary<string, string> GetColunms()
            => new Dictionary<string, string>()
            {
                        { "s.id", "id" },
                        { "s.schedule_time", "scheduleTime" },
                        { "s.person_id", "personId" },
                        { "s.company_id", "companyId" },
                        { "s.create_at", "createAt" },
                        { "s.update_at", "updateAt" }

            };

        private static string WhereClause(NotAvailableScheduleRequestDto dto)
        {
            var build = new StringBuilder();

            build.Append("1 = 1 ");

            if(dto.Id.HasValue)
            {
                build.Append($" and s.id = {dto.Id} ");

                return build.ToString();
            }

            if (dto.PersonId.HasValue)
            {
                build.Append($"and s.person_id = '{dto.PersonId.Value}' ");
            }

            if (dto.CompanyId.HasValue)
            {
                build.Append($"and s.company_id = '{dto.CompanyId.Value}' ");
            }

            if (dto.StartScheduleDate.HasValue)
            {
                build.Append($"and s.schedule_time >= '{new DateTime(dto.StartScheduleDate.Value.Year, dto.StartScheduleDate.Value.Month, dto.StartScheduleDate.Value.Day)}'");
            }
            else
            {
                dto.StartScheduleDate = ScheduleDate.GenerateCurrentDate();
                build.Append($"and s.schedule_time >= '{dto.StartScheduleDate}'");
            }

            if (dto.EndScheduleDate.HasValue)
            {
                build.Append($"and s.schedule_time <= '{new DateTime(dto.EndScheduleDate.Value.Year, dto.EndScheduleDate.Value.Month, dto.EndScheduleDate.Value.Day,23,59,59)}'");
            }
            else
            {
                build.Append($"and s.schedule_time <= '{new DateTime(dto.StartScheduleDate.Value.Year, dto.StartScheduleDate.Value.Month, dto.StartScheduleDate.Value.Day,23,59,59)}'");
            }
            return build.ToString();
        }
    }
}
