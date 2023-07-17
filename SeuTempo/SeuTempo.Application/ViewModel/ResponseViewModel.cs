namespace SeuTempo.Application.ViewModel
{
    public class ResponseViewModel<T>
    {
        public string? Mensagem { get; set; }
        public int Codigo { get; set; }
        public bool Sucesso { get; set; }
        public T? Dados { get; set; }
    }
}
