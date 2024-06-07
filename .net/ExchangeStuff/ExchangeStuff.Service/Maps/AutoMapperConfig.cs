using AutoMapper;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Models.Images;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Models.Rating;

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
            
            #region Mapper Comment
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Comment, CreateCommentModel>().ReverseMap();
            //CreateMap<Comment, UpdateCommentModel>().ReverseMap();
            #endregion

            #region Mapper Image
            CreateMap<Image, ImageViewModel>().ReverseMap();
            #endregion

            #region Mapper rating
            CreateMap<Rating, RatingViewModel>().ReverseMap();
            #endregion
        }
    }
}
