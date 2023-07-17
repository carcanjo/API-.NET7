using Dapper;
using SeuTempo.Core.Entities.UsuarioAPI;
using SeuTempo.Core.Interfaces;
using SeuTempo.Infrastructure.Context;
using System.Data;

namespace SeuTempo.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly SeuTempoDbContext _context;
        public TokenRepository(SeuTempoDbContext context) => _context = context;

        public async Task<UsuarioApi?> ListarUsuarioApiAsync(Guid login, string senha)
        {
            return await _context.DbConnection.QueryFirstOrDefaultAsync<UsuarioApi?>(
                    sql: "[autenticacao].[sp_ListarUsuarioApi]",
                    param: new
                    {
                        login,
                        senha
                    },
                    commandType: CommandType.StoredProcedure
                );
        }
    }
}
