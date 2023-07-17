namespace SeuTempo.Application.Interfaces
{
    public interface IApiConfigService
    {
        public string? AplicationJson { get; set; }

        public string? NomeRequest { get; set; }
        public int TimeOut { get; set; }
    }
}
