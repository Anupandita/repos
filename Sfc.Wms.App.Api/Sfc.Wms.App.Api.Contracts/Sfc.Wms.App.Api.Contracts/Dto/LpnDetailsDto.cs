using System.Collections.Generic;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Contracts.Dto
{
    public class LpnDetailsDto
    {
        public List<CaseDetailDto> CaseDetailsDto { set; get; }
        
        public List<VendorDto> VendorListDto { set; get; }
    }
}
