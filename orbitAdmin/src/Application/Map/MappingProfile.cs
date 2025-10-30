using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Blocks;
using SchoolV01.Domain.Entities.Pages;
using SchoolV01.Shared.ViewModels.Articles;
using SchoolV01.Shared.ViewModels.Blocks;
using SchoolV01.Shared.ViewModels.Events;
using SchoolV01.Shared.ViewModels.Menus;
using SchoolV01.Shared.ViewModels.Pages;
using SchoolV01.Shared.ViewModels.Settings;

namespace SchoolV01.Application.Map
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //Blocks
            CreateMap<BlockCategory, BlockCategoryViewModel>();
            CreateMap<BlockCategoryInsertModel, BlockCategory>();
            CreateMap<BlockCategoryUpdateModel, BlockCategory>();

            CreateMap<Block, BlockViewModel>();
            CreateMap<Block, BlockEndpointViewModel>();
            //.ForMember(dest => dest.FileUrl, opt => opt.MapFrom<CustomBlockFileResolver>());
            CreateMap<BlockInsertModel, Block>();
            CreateMap<BlockUpdateModel, Block>();

            CreateMap<BlockPhoto, BlockPhotoViewModel>();
            CreateMap<BlockPhotoInsertModel, BlockPhoto>();
            CreateMap<BlockPhotoUpdateModel, BlockPhoto>();

            CreateMap<BlockSeo, BlockSeoViewModel>();
            CreateMap<BlockSeoInsertModel, BlockSeo>();
            CreateMap<BlockSeoUpdateModel, BlockSeo>();



            CreateMap<PageSeo, PageSeoViewModel>();
            CreateMap<PageSeoInsertModel, PageSeo>();
            CreateMap<PageSeoUpdateModel, PageSeo>();

            CreateMap<BlockAttachement, BlockAttachementViewModel>();
            CreateMap<BlockAttachementInsertModel, BlockAttachement>();
            CreateMap<BlockAttachementUpdateModel, BlockAttachement>();

            //Menus
            CreateMap<MenuCategory, MenuCategoryViewModel>();
            CreateMap<MenuCategoryInsertModel, MenuCategory>();
            CreateMap<MenuCategoryUpdateModel, MenuCategory>();


            CreateMap<Menu, MenuViewModel>();
            CreateMap<MenuInsertModel, Menu>();
            CreateMap<MenuUpdateModel, Menu>();

            //Pages
            CreateMap<Page, PageViewModel>();
            
            CreateMap<PageInsertModel, Page>();
            CreateMap<PageUpdateModel, Page>();

            CreateMap<PagePhoto, PagePhotoViewModel>();
            CreateMap<PagePhotoInsertModel, PagePhoto>();
            CreateMap<PagePhotoUpdateModel, PagePhoto>();
            CreateMap<PageAttachement, PageAttachementViewModel>();
            CreateMap<PageAttachementInsertModel, PageAttachement>();
            CreateMap<PageAttachementUpdateModel, PageAttachement>();

            //Events
            CreateMap<EventCategory, EventCategoryViewModel>();
            CreateMap<EventCategoryInsertModel, EventCategory>();
            CreateMap<EventCategoryUpdateModel, EventCategory>();

            CreateMap<Event, EventViewModel>();
            CreateMap<EventInsertModel, Event>();
            CreateMap<EventUpdateModel, Event>();

            CreateMap<EventPhoto, EventPhotoViewModel>();
            CreateMap<EventPhotoInsertModel, EventPhoto>();
            CreateMap<EventPhotoUpdateModel, EventPhoto>();

            CreateMap<EventAttachement, EventAttachementViewModel>();
            CreateMap<EventAttachementInsertModel, EventAttachement>();
            CreateMap<EventAttachementUpdateModel, EventAttachement>();



        }

    }

    //public static class Mapping
    //{
    //    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() => {
    //        var configuration = new MapperConfiguration(config => {
    //            config.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
    //            config.AddProfile<MappingProfile>();
    //        });
    //        return configuration.CreateMapper();
    //    });

    //    public static IMapper Mapper => Lazy.Value;
    //}
    //public class CustomBlockImageResolver : IValueResolver<Block, BlockViewModel, string>
    //{
    //    public string Resolve(Block source, BlockViewModel destination, string member, ResolutionContext context)
    //    {
    //        return source.Url + Path.DirectorySeparatorChar.ToString() + source.Image;
    //    }
    //}

    //public class CustomBlockFileResolver : IValueResolver<Block, BlockViewModel, string>
    //{
    //    public string Resolve(Block source, BlockViewModel destination, string member, ResolutionContext context)
    //    {
    //        return source.Url + Path.DirectorySeparatorChar.ToString() + source.File;
    //    }
    //}

}


