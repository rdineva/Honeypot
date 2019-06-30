using AutoMapper;

namespace Honeypot
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

            return config;
        }
    }
}