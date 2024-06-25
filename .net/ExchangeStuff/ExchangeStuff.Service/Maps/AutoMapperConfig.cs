using AutoMapper;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.Admins;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Models.Campuses;
using ExchangeStuff.Service.Models.Categories;
using ExchangeStuff.Service.Models.Comments;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Models.Images;
using ExchangeStuff.Service.Models.Moderators;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.PostTicket;
using ExchangeStuff.Service.Models.ProductBanReports;
using ExchangeStuff.Service.Models.Products;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Models.Rating;
using ExchangeStuff.Service.Models.Resources;
using ExchangeStuff.Service.Models.Tokens;
using ExchangeStuff.Service.Models.TransactionHistory;
using ExchangeStuff.Service.Models.UserBanReports;
using ExchangeStuff.Service.Models.Users;

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
            CreateMap<Rating, CreateCommentModel>().ReverseMap();
            CreateMap<Rating, UpdateRatingModel>().ReverseMap();
            #endregion
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<PermissionGroup, PermissionGroupDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<ExchangeStuff.Core.Entities.Action, ActionDTO>().ReverseMap();
            CreateMap<Resource, ResourceDTO>().ReverseMap();
            CreateMap<Permission, PermissionViewModel>().ReverseMap();
            CreateMap<PermissionGroup, PermisisonGroupViewModel>().ReverseMap();
            CreateMap<Core.Entities.Action, ActionViewModel>().ReverseMap();
            CreateMap<Resource, ResourceViewModel>().ReverseMap();
            CreateMap<Admin, AdminViewModel>().ReverseMap();
            CreateMap<Product, CreateProductModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<UserBalance, UserBalanceViewModel>().ReverseMap();
            CreateMap<Campus, CampusViewModel>().ReverseMap();
            CreateMap<FinancialTicket, FinancialTicketViewModel>().ReverseMap();
            CreateMap<FinancialTicket,CreateFinancialTicketModel>().ReverseMap();
            CreateMap<FinancialTicket, UpdateFinancialTicketModel>().ReverseMap();
            CreateMap<TransactionHistory, CreateTransactionHistoryModel>().ReverseMap();
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<PostTicket, PostTicketViewModel>().ReverseMap();
            CreateMap<PurchaseTicket, CreatePurchaseTicketModel>().ReverseMap();
            CreateMap<PurchaseTicket, PurchaseTicketViewModel>().ReverseMap();
            CreateMap<PurchaseTicket, CreatePurchaseTicketModel>().ReverseMap();
            CreateMap<TransactionHistory, CreateTransactionHistoryModel>().ReverseMap();
            CreateMap<TransactionHistory, TransactionHistoryViewModel>().ReverseMap();
            CreateMap<BanReason, BanReasonViewModel>().ReverseMap();
            CreateMap<UserBanReport, UserBanReportViewModel>().ReverseMap();
            CreateMap<ProductBanReport, ProductBanReportViewModel>().ReverseMap();
            CreateMap<Token, TokenViewModel>().ReverseMap();
            CreateMap<Moderator, ModeratorViewModel>().ReverseMap();
            CreateMap<ProductImageUserViewModel, Product>().ReverseMap();
            //CreateMap(typeof(PaginationItem<>), typeof(PaginationItem<>)).ConvertUsing(typeof(PaginationItemConverter<,>));
        }
    }

}
