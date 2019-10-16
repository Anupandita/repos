using DataGenerator;
using DataGenerator.Sources;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fakes
{
    public class RolesDataGeneratorProfile : MappingProfile<RolesDto>
    {
        public override void Configure()
        {
            Property(el => el.RoleId).DataSource<IntegerSource>();
            Property(el => el.RoleName).DataSource<NameSource>();
            Property(el => el.RoleDesc).DataSource<CitySource>();
        }
    }
}