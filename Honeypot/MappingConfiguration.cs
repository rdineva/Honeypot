using AutoMapper;
using Honeypot.Models;
using Honeypot.ViewModels;

namespace Honeypot
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<RegisterViewModel, HoneypotUser>();
        }
    }
}
