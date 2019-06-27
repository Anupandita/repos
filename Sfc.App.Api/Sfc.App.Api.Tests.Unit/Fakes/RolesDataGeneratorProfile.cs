using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
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