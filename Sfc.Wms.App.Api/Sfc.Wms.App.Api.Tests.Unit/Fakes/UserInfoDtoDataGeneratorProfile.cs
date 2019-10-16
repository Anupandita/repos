using DataGenerator;
using DataGenerator.Sources;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fakes
{
    public class UserInfoDtoDataGeneratorProfile : MappingProfile<UserInfoDto>
    {
        public override void Configure()
        {
            Property(el => el.Token).DataSource<GuidSource>();
            Property(el => el.UserName).DataSource<NameSource>();
            Property(el => el.FirstName).DataSource<NameSource>();
            Property(el => el.LastName).DataSource<NameSource>();
            Property(el => el.UserId).DataSource<IntegerSource>();
            Property(el => el.Permissions).List<PermissionsDto>();
            Property(el => el.Preferences).List<PreferencesDto>();
            Property(el => el.Menus).List<MenusDto>();
        }
    }
}