using DataGenerator;
using DataGenerator.Sources;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class PreferencesDataGeneratorProfile : MappingProfile<PreferencesDto>
    {
        public override void Configure()
        {
            Property(el => el.Id).DataSource<IntegerSource>();
            Property(el => el.SettingId).DataSource<IntegerSource>();
            Property(el => el.UnconstrainedValue).DataSource<CompanySource>();
            Property(el => el.UserId).DataSource<IntegerSource>();
        }
    }
}