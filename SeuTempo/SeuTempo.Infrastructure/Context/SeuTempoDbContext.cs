using System.Data;
using System.Data.SqlClient;

namespace SeuTempo.Infrastructure.Context
{
    public class SeuTempoDbContext : IDisposable
    {
        public IDbConnection DbConnection { get; }
        public SeuTempoDbContext() =>
               DbConnection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));

        public void Dispose() => DbConnection?.Dispose();
    }
}
