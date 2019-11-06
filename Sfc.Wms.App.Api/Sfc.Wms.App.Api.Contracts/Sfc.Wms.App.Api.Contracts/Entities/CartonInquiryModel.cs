namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class CartonInquiryModel : PaginationModel
    {
        public string inpt_curr_aisle { get; set; }
        public string inpt_curr_bay { get; set; }
        public string inpt_curr_lvl { get; set; }
        public string inpt_curr_zone { get; set; }
        public string inpt_exact_carton_nbr { get; set; }
        public string inpt_exact_pktctrl_nbr { get; set; }
        public string inpt_from_carton_nbr { get; set; }
        public string inpt_from_pkt_control_nbr { get; set; }
        public string inpt_from_stat { get; set; }
        public string inpt_from_wave_nbr { get; set; }
        public string inpt_masterpack_id { get; set; }
        public string inpt_pallet_id { get; set; }
        public string inpt_pick_aisle { get; set; }
        public string inpt_pick_bay { get; set; }
        public string inpt_pick_lvl { get; set; }
        public string inpt_pick_zone { get; set; }
        public string inpt_pnh_ctrl_nbr { get; set; }
        public string inpt_shortage_type { get; set; }
        public string inpt_shpmt_nbr { get; set; }
        public string inpt_sku_id { get; set; }
        public string inpt_to_carton_nbr { get; set; }
        public string inpt_to_pkt_control_nbr { get; set; }
        public string inpt_to_stat { get; set; }
        public string inpt_to_wave_nbr { get; set; }
        public string inpt_wave_nbr { get; set; }
    }
}