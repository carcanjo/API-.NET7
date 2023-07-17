using SeuTempo.Application.Interfaces;

namespace SeuTempo.Application.Services
{
    public class ApiConfigService : IApiConfigService
    {
        public string? AplicationJson { get; set; }

        public string? NomeRequest { get; set; }
        public int TimeOut { get; set; }
    }
}
