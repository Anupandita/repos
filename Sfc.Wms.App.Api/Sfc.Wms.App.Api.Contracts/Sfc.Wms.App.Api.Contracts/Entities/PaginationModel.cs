namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class PaginationModel
    {
        public int pageNo { get; set; }
        public int rowsPerPage { get; set; }
        public int totalRows { get; set; }
    }
}