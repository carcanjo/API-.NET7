using SeuTempo.Application.ViewModel;

namespace SeuTempo.API.Utils
{
    public class Responses
    {
        public static ResponseViewModel<dynamic> ApplicationSucessMessage(dynamic? dados)
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = "Sucesso",
                Codigo = 200,
                Sucesso = true,
                Dados = dados
            };
        }

        public static ResponseViewModel<dynamic> ApplicationErrorMessage()
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = "Ocorreu um erro interno na aplicação, tente novamente mais tarde",
                Codigo = 500,
                Sucesso = true,
                Dados = new List<string>()
            };
        }

        public static ResponseViewModel<dynamic> ApplicationErrorMessage(string mensagem)
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = mensagem,
                Codigo = 500,
                Sucesso = true,
                Dados = new List<string>()
            };
        }

        public static ResponseViewModel<dynamic> ApplicationManyRequest(string mensagem)
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = mensagem,
                Codigo = 429,
                Sucesso = true,
                Dados = new List<string>()
            };
        }

        public static ResponseViewModel<dynamic> DomainErrorMessage(string mensagem, IReadOnlyCollection<string> errors)
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = mensagem,
                Codigo = 400,
                Sucesso = false,
                Dados = errors
            };
        }

        public static ResponseViewModel<dynamic> DomainErrorMessage(string mensagem)
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = mensagem,
                Codigo = 400,
                Sucesso = false,
                Dados = new List<string>()
            };
        }

        public static ResponseViewModel<dynamic> UnauthorizeErrorMessage()
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = "Não autorizado",
                Codigo = 401,
                Sucesso = false,
                Dados = new List<string>()
            };
        }

        public static ResponseViewModel<dynamic> ForbiddenErrorMessage()
        {
            return new ResponseViewModel<dynamic>
            {
                Mensagem = "Proibido acesso",
                Codigo = 403,
                Sucesso = false,
                Dados = new List<string>()
            };
        }
    }
}
