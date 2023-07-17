namespace SeuTempo.Core.Exceptions
{
    public class DomainException : Exception
    {
        internal List<string> _error;
        public IReadOnlyCollection<string> Error => _error;
        public DomainException() => _error = new List<string>();
        public DomainException(string mensagem, List<string> errors) : base(mensagem) => _error = errors;
        public DomainException(string mensagem) : base(mensagem) => _error = new List<string>();
        public DomainException(string mensagem, Exception exception) : base(mensagem) => _error = new List<string>();
    }
}
