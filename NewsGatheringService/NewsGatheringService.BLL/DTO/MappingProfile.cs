using AutoMapper;
using NewsGatheringService.DAL.Entities;
using System;

namespace NewsGatheringService.BLL.DTO
{
    /// <summary>
    /// Class for mapping database entities with DTO entities
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<News, NewsDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Subcategory, opt => opt.MapFrom(src => "" ?? src.Subcategory.Name))
                .ForMember(dest => dest.NewsHeaderImage, opt => opt.MapFrom(src => Convert.ToBase64String(src.NewsHeaderImage)));
            
            CreateMap<NewsStructure, NewsStructureDTO>();
            
            CreateMap<NewsUrl, NewsUrlDTO>();
            
            CreateMap<User, UserDTO>();
            
            CreateMap<Category, CategoryDTO>();
            
            CreateMap<Subcategory, SubcategoryDTO>();
        }
    }
}
