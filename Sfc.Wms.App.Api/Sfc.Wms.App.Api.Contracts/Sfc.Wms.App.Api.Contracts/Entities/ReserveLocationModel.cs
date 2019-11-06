namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ReserveLocationModel : PaginationModel
    {
        public string user_inpt_aisle { set; get; }
        public string user_inpt_grp_type { set; get; }
        public string user_inpt_locn_cls { set; get; }
        public string user_inpt_locn_grp { set; get; }
        public string user_inpt_lvl { set; get; }
        public string user_inpt_putwy_zone { get; set; }
        public string user_inpt_slot { set; get; }
        public string user_inpt_zone { set; get; }
    }
}