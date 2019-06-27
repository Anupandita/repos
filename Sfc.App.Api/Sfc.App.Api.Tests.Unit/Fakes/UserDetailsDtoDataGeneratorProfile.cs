using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class UserDetailsDtoDataGeneratorProfile : MappingProfile<UserDetailsDto>
    {
        public override void Configure()
        {
            Property(el => el.token).DataSource<GuidSource>();

            Property(el => el.userName).DataSource<NameSource>();

            Property(el => el.firstName).DataSource<NameSource>();

            Property(el => el.lastName).DataSource<NameSource>();

            Property(el => el.userName).DataSource<IntegerSource>();
        }
    }
}