using System;
using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using Sfc.App.Gateways;
using Sfc.App.Interfaces;
using Sfc.Wms.Cache.InMemory;
using Sfc.Wms.Security.Rbac.App.AutoMapper;
using Sfc.Wms.Security.Rbac.App.Gateway;
using Sfc.Wms.Security.Rbac.App.Interfaces;
using Sfc.Wms.Security.Rbac.App.Services;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;
using Sfc.Wms.Security.Rbac.Contracts.Interfaces;
using Sfc.Wms.Security.Rbac.Repository.Context;
using Sfc.Wms.Security.Rbac.Repository.Dtos;
using Sfc.Wms.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Security.Rbac.Repository.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Sfc.App.Api.App_Start
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            RegisterTypes(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
#if DEBUG
            container.Verify();
#endif
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            return container;
        }


        private static void RegisterTypes(Container container)
        {
            container.RegisterSingleton<IMapper>(() =>
            {
                var cc = typeof(SfcMapper);
                var mapper = new Mapper(new MapperConfiguration(c =>
                {
                    c.AddProfile(new RolesDtoMapping1());
                    //SfcMapper.Initialize(c);
                }));
#if DEBUG
                mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                return mapper;
            });

            container.Register(() =>
                    new SfcRbacContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString)
                , Lifestyle.Singleton);
            container.Register<IRabcGateway, RbacGateway>();
            container.Register<IUserRabcService, UserRabcService>();
            container.Register<IUserRabcGateway, UserRabcGateway>();
            container.Register<IUserAuthenticationGateway, UserAuthenticationGateway>();
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default));
        }

        private static void Initialize(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            throw new NotImplementedException();
        }
    }

    internal class RolesDtoMapping1 : Profile
    {
        public RolesDtoMapping1()
        {
            CreateMap<SwmRoles, RolesDto>();
            CreateMap<SwmPermissions, PermissionsDto>()
                .ForMember(d => d.PermissionId, s => s.MapFrom(_ => _.PermissionId))
                .ForMember(d => d.Name, s => s.MapFrom(_ => _.Name))
                .ForAllOtherMembers(el => el.Ignore());
            CreateMap<SwmUserSetting, PreferencesDto>();

            CreateMap<SwmMenus, MenusDto>()
                .ForMember(d => d.MenuId, s => s.MapFrom(_ => _.MenuId))
                .ForMember(d => d.MenuName, s => s.MapFrom(_ => _.MenuName))
                .ForMember(d => d.MenuUrl, s => s.MapFrom(_ => _.MenuUrl))
                .IncludeAllDerived()
                .ForAllOtherMembers(el => el.Ignore());

            CreateMap<SwmMenus, MenuDetailsDto>()
                .ForMember(d => d.MenuId, s => s.MapFrom(_ => _.MenuId))
                .ForMember(d => d.MenuName, s => s.MapFrom(_ => _.MenuName))
                .ForMember(d => d.MenuUrl, s => s.MapFrom(_ => _.MenuUrl))
                .ForAllOtherMembers(el => el.Ignore());

            CreateMap<UserInfoDto, UserDetailsDto>()
                .ForMember(d => d.userName, s => s.MapFrom(_ => _.UserName))
                .ForMember(d => d.firstName, s => s.MapFrom(_ => _.FirstName))
                .ForMember(d => d.lastName, s => s.MapFrom(_ => _.LastName))
                .ForMember(d => d.token, s => s.MapFrom(_ => _.Token))
                .ForMember(d => d.userId, s => s.MapFrom(_ => _.UserId))
                .ForMember(d => d.menus, s => s.MapFrom(_ => _.Menus))
                .ForMember(d => d.userPermissions, s => s.MapFrom(_ => _.Permissions))
                .ForMember(d => d.userPreferences, s => s.MapFrom(_ => _.Preferences))
                .IncludeAllDerived()
                .ForAllOtherMembers(_ => _.Ignore());

            CreateMap<PreferencesDto, Preferences>()
                .ForMember(d => d.USER_ID, s => s.MapFrom(_ => _.UserId))
                .ForMember(d => d.SETTING_ID, s => s.MapFrom(_ => _.SettingId))
                .ForMember(d => d.UNCONSTRAINED_VALUE, s => s.MapFrom(_ => _.UnconstrainedValue))
                .ForAllOtherMembers(_ => _.Ignore());

            CreateMap<PermissionsDto, Permissions>()
                .ForMember(d => d.NAME, s => s.MapFrom(_ => _.Name))
                .ForMember(d => d.ROLE_ID, s => s.MapFrom(_ => _.RoleId))
                .ForMember(d => d.PERMISSION_ID, s => s.MapFrom(_ => _.PermissionId))
                .ForAllOtherMembers(_ => _.Ignore());

            CreateMap<MenusDto, Menu>()
                .ForMember(d => d.menuId, s => s.MapFrom(_ => _.MenuId))
                .ForMember(d => d.displayName, s => s.MapFrom(_ => _.MenuName))
                .ForMember(d => d.routeTo, s => s.MapFrom(_ => _.MenuUrl))
                .ForMember(d => d.children, s => s.PreCondition(_ => _.ChildMenus.Count > 0))
                .ForMember(d => d.children, s => s.MapFrom(_ => _.ChildMenus))
                .IncludeAllDerived()
                .ForAllOtherMembers(_ => _.Ignore());

            CreateMap<MenuDetailsDto, MenuDetails>()
                .ForMember(d => d.menuId, s => s.MapFrom(_ => _.MenuId))
                .ForMember(d => d.displayName, s => s.MapFrom(_ => _.MenuName))
                .ForMember(d => d.routeTo, s => s.MapFrom(_ => _.MenuUrl))
                .ForAllOtherMembers(_ => _.Ignore());

            CreateMap<SwmUserMaster, UserInfoDto>()
                .ForMember(d => d.UserName, s => s.MapFrom(_ => _.UserName))
                .ForMember(d => d.UserId, s => s.MapFrom(_ => _.UserId))
                .ForMember(d => d.FirstName, s => s.MapFrom(_ => _.FirstName))
                .ForMember(d => d.LastName, s => s.MapFrom(_ => _.LastName))
                // .ForMember(d => d.LastName, s => s.MapFrom(_ => _.LastName))
                .IncludeAllDerived()
                .ForAllOtherMembers(el => el.Ignore());
        }
    }
}