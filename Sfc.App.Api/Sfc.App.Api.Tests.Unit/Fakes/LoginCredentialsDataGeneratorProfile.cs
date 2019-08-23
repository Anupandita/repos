using DataGenerator;
using DataGenerator.Sources;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fakes
{
    public class LoginCredentialsDataGeneratorProfile : MappingProfile<LoginCredentials>
    {
        public override void Configure()
        {
            Property(el => el.UserName).DataSource<NameSource>();
            Property(el => el.Password).DataSource<PasswordSource>();
        }
    }
}