using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class SwmMessageSourceDtoProfile : MappingProfile<SwmMessageSourceDto>
    {
        public override void Configure()
        {
            Property(p => p.SourceId).DataSource<IntegerSource>();
            Property(p => p.SourceName).DataSource<NameSource>();
            Property(p => p.CreateDateTime).DataSource<DateTimeSource>();
            Property(p => p.UpdatedBy).DataSource<DateTimeSource>();
            Property(p => p.CreatedBy).DataSource<NameSource>();
            Property(p => p.UpdatedBy).DataSource<NameSource>();
        }
    }
}