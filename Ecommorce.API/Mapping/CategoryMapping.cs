using AutoMapper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Product;

namespace Ecommorce.API.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateDTO, Category>().ReverseMap();
        }
    }
}
