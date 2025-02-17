using AutoMapper;
using UpriseTask.Entities;
using UpriseTask.Mappings.DTO;
using UpriseTask.Mappings.Profiler;

namespace UpriseTask.Mappings
{
    public static class PlantMapper
    {
        private static readonly IMapper MapperInstance = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }).CreateMapper();

        public static SolarPlantEntity MapBusinessObjectToEntity(this SolarPlantDto model)
        {
            return MapperInstance.Map<SolarPlantEntity>(model);
        }

        public static SolarPlantProductionEntity MapBusinessObjectToEntity(this SolarPlantProductionDto model)
        {
            return MapperInstance.Map<SolarPlantProductionEntity>(model);
        }

        public static void MapBusinessObjectToEntity(this SolarPlantDto model, SolarPlantEntity entity)
        {
            MapperInstance.Map(model, entity);
        }

        public static SolarPlantDto MapEntityToBusinessObject(this SolarPlantEntity entity)
        {
            return MapperInstance.Map<SolarPlantDto>(entity);
        }

        public static SolarPlantProductionDto MapEntityToBusinessObject(this SolarPlantProductionEntity entity)
        {
            return MapperInstance.Map<SolarPlantProductionDto>(entity);
        }

    }
}
