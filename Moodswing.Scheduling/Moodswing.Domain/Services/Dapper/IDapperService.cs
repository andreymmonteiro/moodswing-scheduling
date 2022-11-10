using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Domain.Services.Dapper
{
    public interface IDapperService<TClass> where TClass : class
    {
        Task<IEnumerable<TClass>> GetByParamsAsync(
            string tableName,
            Dictionary<string, string> columns,
            string where = default,
            List<string> excludeValues = default,
            int? take = default,
            int? skip = default,
            string joins = "");
    }
}
