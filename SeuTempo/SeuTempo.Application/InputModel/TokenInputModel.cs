namespace SeuTempo.Application.InputModel
{
    public class TokenInputModel
    {
        public required Guid ClienteId { get; set; }
        public required string Senha { get; set; }
    }
}
