using AutoMapper;
using NewsGatheringService.DAL.Entities;

namespace NewsGatheringService.BLL.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<News, NewsDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Subcategory, opt => opt.MapFrom(src => src.Subcategory.Name));
            CreateMap<NewsStructure, NewsStructureDTO>();
        }
    }
}
