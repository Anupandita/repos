using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class PickLocationDtlDtoProfile : MappingProfile<PickLocationDtlDto>
    {
        public override void Configure()
        {
           
            Property(p => p.ActualQty).DataSource<IntegerSource>();
            Property(p => p.LocationId).DataSource<GuidSource>();
            Property(p => p.SkuId).DataSource<GuidSource>();
            Property(p => p.ToBeFilledCases).DataSource<IntegerSource>();
            Property(p => p.ToBeFilledQty).DataSource<DecimalSource>();
            Property(p => p.LocationSequenceNbr).DataSource<DecimalSource>();
            Property(p => p.UserId).DataSource<NameSource>();
        }
    }
}