using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class PermissionDataGeneratorProfile : MappingProfile<PermissionsDto>
    {
        public override void Configure()
        {
            Property(el => el.Name).DataSource<IntegerSource>();
            Property(el => el.Name).DataSource<NameSource>();
            Property(el => el.PermissionId).DataSource<IntegerSource>();
        }
    }
}