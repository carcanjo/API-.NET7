using SeuTempo.Core.Entities.UsuarioAPI;

namespace SeuTempo.Core.Interfaces
{
    public interface ITokenRepository
    {
        Task<UsuarioApi?> ListarUsuarioApiAsync(Guid login, string senha);
    }
}
