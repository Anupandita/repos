using AutoMapper;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;

namespace Sfc.Wms.App.App.AutoMapper
{
    public class PrinterValuesMapper
    {
        public static void CreateMaps(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<SysCodeDto, SfcPrinterSelectList>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.CodeId))
                .ForMember(d => d.Description, s => s.MapFrom(p => p.CodeDesc))
                .ForMember(d => d.DisplayName, s => s.MapFrom(p => p.MiscFlag))
                .IncludeAllDerived().ForAllOtherMembers(el => el.Ignore());
        }
    }
}
