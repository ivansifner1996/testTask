using AutoMapper;
using UpriseTask.Entities;
using UpriseTask.Mappings.DTO;

namespace UpriseTask.Mappings.Profiler
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SolarPlantDto, SolarPlantEntity>().ReverseMap();
            CreateMap<SolarPlantProductionDto, SolarPlantProductionEntity>().ReverseMap();
        }
    }
}
