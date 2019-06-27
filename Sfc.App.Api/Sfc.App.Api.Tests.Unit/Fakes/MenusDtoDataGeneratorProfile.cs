using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class MenuDetailsDtoDataGeneratorProfile : MappingProfile<MenuDetailsDto>
    {
        public override void Configure()
        {
            Property(el => el.MenuId).DataSource<IntegerSource>();

            Property(el => el.MenuName).DataSource<NameSource>();

            Property(el => el.MenuUrl).DataSource<WebsiteSource>();
        }
    }

    public class MenusDtoDataGeneratorProfile : MappingProfile<MenusDto>
    {
        public override void Configure()
        {
            Property(el => el.MenuId).DataSource<IntegerSource>();

            Property(el => el.MenuName).DataSource<NameSource>();

            Property(el => el.MenuUrl).DataSource<WebsiteSource>();

            Property(el => el.ChildMenus).List<MenuDetailsDto>();
        }
    }
}