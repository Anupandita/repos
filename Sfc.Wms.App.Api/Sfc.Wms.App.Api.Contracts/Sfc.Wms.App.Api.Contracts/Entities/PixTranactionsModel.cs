namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class PixTranactionsModel : PaginationModel
    {
        public string inpt_end_date { get; set; }
        public string inpt_itemid { get; set; }
        public string inpt_nbr_days { get; set; }
        public string inpt_start_date { get; set; }
    }
}