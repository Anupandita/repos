using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class EmsToWmsDtoProfile : MappingProfile<EmsToWmsDto>
    {
        public override void Configure()
        {
            Property(p => p.Process).DataSource<NameSource>();
            Property(p => p.MessageKey).DataSource<IntegerSource>();
            Property(p => p.AddDate).DataSource<DateTimeSource>();
            Property(p => p.AddWho).DataSource<NameSource>();
            Property(p => p.MessageText).DataSource<NameSource>();
            Property(p => p.ProcessedDate).DataSource<DateTimeSource>();
            Property(p => p.ResponseCode).DataSource<DecimalSource>();
            Property(p => p.Status).DataSource(new[]
            {
                RecordStatus.Ready.ToString(),
                RecordStatus.Processed.ToString(), RecordStatus.Error.ToString()
            });
            Property(p => p.Transaction).DataSource<NameSource>();
        }
    }
}