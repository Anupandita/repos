namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ReturnReceivingInsertModel : PaginationModel
    {
        public string procedureCall { get; set; }
        public string shpmtNbr { get; set; }
        public string workstationId { get; set; }
    }
}