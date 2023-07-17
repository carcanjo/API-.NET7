using SeuTempo.Application.ViewModel;
using SeuTempo.Core.Enuns;

namespace SeuTempo.Application.Interfaces
{
    public interface IHttpClientService<T>
    {
        Task<ResponseHttpViewModel<T>> RequestHttpAsync(string url, bool autenticacao, ProtocoloHttp protocoloHttp,
            dynamic payload, string aplicacao, long? id = null);
    }
}
