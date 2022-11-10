using Dapper;
using Microsoft.Extensions.Configuration;
using Moodswing.Domain.Services.Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Moodswing.Service.Dapper
{
    public sealed class DapperService<TClass> : IDisposable, IDapperService<TClass> where TClass : class
    {
        private readonly string _connection;

        private const string SECTION = "Default";
        private const string EXCLUDE_VALUES_EXCEPTION = "Query can't be executed, checks your columns";
        private const string DOT = ".";
        private const string GENERIC_TABLE_NICK = "z";

        public DapperService(IConfiguration Configuration)
            => _connection = Configuration.GetConnectionString(SECTION);

        public async Task<IEnumerable<TClass>> GetByParamsAsync(
            string tableName,
            Dictionary<string, string> columns,
            string where = default,
            List<string> excludeValues = default,
            int? take = default,
            int? skip = default,
            string joins = "")
        {
            using var db = GetConnection();
            var sql = CreateQuery(tableName, columns, where, excludeValues, take, skip, joins);
            return await db.QueryAsync<TClass>(sql);
        }

        private DbConnection GetConnection()
            => new NpgsqlConnection(_connection);

        private static string CreateQuery(string tableName, Dictionary<string, string> columns, string where, List<string> excludeValues, int? take, int? skip, string joins)
        {
            if (excludeValues is not null)
            {
                foreach (var excludeValue in CollectionsMarshal.AsSpan(excludeValues))
                {
                    if (columns.Any(column => column.Equals(excludeValue)))
                    {
                        throw new ArgumentException(EXCLUDE_VALUES_EXCEPTION);
                    }
                };
            }

            var columnsResult = PrepareColumnsAsParallel(columns);

            var query = $"SELECT {columnsResult} FROM {tableName} as  {GetNickTable(columns.FirstOrDefault().Key)} {joins} ";

            var whereClause = string.Empty;

            if (!string.IsNullOrWhiteSpace(where))
            {
                whereClause = string.Format($"WHERE {where} ");
            }
            var takeQuery = $"LIMIT {take ?? 100}";
            var skipQuery = $"OFFSET {skip ?? 0}";

            return $"{query} {whereClause} {takeQuery} {skipQuery}";
        }

        private static string PrepareColumnsAsParallel(Dictionary<string, string> columns)
            => string.Join(", ", columns.AsParallel().Select(column =>
            column.Key.Contains(DOT) ?
                $" {column.Key.Replace(" ", default)}  as {column.Value}" :
                $" {GENERIC_TABLE_NICK}{DOT}{column.Key.Replace(" ", default)}  as {column.Value}"));

        private static string GetNickTable(string column)
            => column.Contains(DOT) ?
                column[default..(column.IndexOf(DOT))] :
                $"{GENERIC_TABLE_NICK}";

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
