using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.DematicMessage.Contracts.Dto;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class CostDtoMapping : MappingProfile<CostDto>
    {
        public override void Configure()
        {
            Property(p => p.StorageClassAttribute1).DataSource<GuidSource>();
            Property(p => p.StorageClassAttribute2).DataSource<IntegerSource>();
           
        }
    }
}