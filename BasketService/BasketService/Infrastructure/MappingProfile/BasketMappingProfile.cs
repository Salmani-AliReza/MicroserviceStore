using AutoMapper;
using BasketService.Model.Entities;
using BasketService.Model.Services;

namespace BasketService.Infrastructure.MappingProfile
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<BasketItem, AddItemToBasketDto>().ReverseMap();
        }
    }
}
