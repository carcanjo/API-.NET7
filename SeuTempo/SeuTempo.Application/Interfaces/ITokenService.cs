using SeuTempo.Application.InputModel;
using SeuTempo.Application.ViewModel;

namespace SeuTempo.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenViewModel> GetTokenAsync(TokenInputModel tokenInputModel);
    }
}
