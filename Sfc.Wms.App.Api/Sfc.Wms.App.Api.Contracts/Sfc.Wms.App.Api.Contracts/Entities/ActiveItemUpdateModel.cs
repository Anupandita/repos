// ReSharper disable InconsistentNaming
namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ActiveItemUpdateModel
    {
        public string locn_id { get; set; }
        public string max_invn_cases { get; set; }
        public string max_invn_qty { get; set; }
        public string min_invn_cases { get; set; }
        public string min_invn_qty { get; set; }
        public string seq_nbr { get; set; }
    }
}