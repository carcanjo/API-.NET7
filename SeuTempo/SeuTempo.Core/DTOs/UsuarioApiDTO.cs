namespace SeuTempo.Core.DTOs
{
    public class UsuarioApiDTO
    {
        public long Id { get; set; }
        public Guid? Login { get; set; }
        public string? Senha { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
