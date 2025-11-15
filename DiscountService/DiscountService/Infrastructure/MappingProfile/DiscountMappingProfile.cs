using AutoMapper;
using DiscountService.Model.Entites;
using DiscountService.Model.Services;

namespace DiscountService.Infrastructure.MappingProfile
{
    public class DiscountMappingProfile : Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<DiscountCode, DiscountDto>().ReverseMap();
        }
    }
}
