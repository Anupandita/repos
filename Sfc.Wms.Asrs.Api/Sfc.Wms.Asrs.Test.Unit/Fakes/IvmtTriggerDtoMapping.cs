using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class IvmtTriggerDtoMapping : MappingProfile<IvmtTriggerInputDto>
    {
        public override void Configure()
        {
            Property(p => p.ContainerId).DataSource<NameSource>();
            Property(p => p.Sku).DataSource<IntegerSource>();
           
            
        }
    }
}