using SeuTempo.Application.Interfaces;
using System.Text.Json;

namespace SeuTempo.Application.Services
{
    public class UltilService : IUltilService
    {
        public T ConverterObjeto<T>(dynamic valor)
        {
            var objetoSerialize = JsonSerializer.Serialize(valor);
            return JsonSerializer.Deserialize<T>(objetoSerialize);
        }
    }
}
