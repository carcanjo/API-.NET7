namespace SeuTempo.Application.ViewModel
{
    public class ResponseHttpViewModel<T>
    {
        public T? Dados { get; set; }
        public bool Successo { get; set; } = false;
        public string? Mensagem { get; set; } 
    }
}
