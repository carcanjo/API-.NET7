namespace SeuTempo.Core.Entities.UsuarioAPI
{
    public class UsuarioApi : Base
    {
        public Guid? Login { get; private set; }
        public string? Senha { get; private set; }
        public bool Status { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public override bool Validate() => throw new NotImplementedException();
    }
}
