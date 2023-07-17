using AutoMapper;

namespace SeuTempo.Test.Config
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetConfiguration()
        {
            var autoMapperConfig = new MapperConfiguration(x =>
            {
                x.CreateMap<string, string>();
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
