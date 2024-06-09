using AutoMapper;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.Categories;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Models.Resources;

namespace ExchangeStuff.Service.Maps
{
    public class AutoMapperConfig
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(x =>
            {
                x.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod!.IsAssembly;
                x.AddProfile<MapperHandler>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class MapperHandler : Profile
    {
        public MapperHandler()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<PermissionGroup, PermissionGroupDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<ExchangeStuff.Core.Entities.Action, ActionDTO>().ReverseMap();
            CreateMap<Resource,ResourceDTO>().ReverseMap();
            CreateMap<Permission,PermissionViewModel>().ReverseMap();
            CreateMap<PermissionGroup, PermisisonGroupViewModel>().ReverseMap();
            CreateMap<Core.Entities.Action, ActionViewModel>().ReverseMap();
            CreateMap<Resource, ResourceViewModel>().ReverseMap();
        }
    }
}
