using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class UserMenusDtoDataGeneratorProfile : MappingProfile<MenusDto>
    {
        public override void Configure()
        {
            Property(el => el.MenuName).DataSource<NameSource>();

            Property(el => el.ChildMenus).List<MenuDetailsDto>();
        }
    }
}