using DataGenerator;
using DataGenerator.Sources;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fakes
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
}