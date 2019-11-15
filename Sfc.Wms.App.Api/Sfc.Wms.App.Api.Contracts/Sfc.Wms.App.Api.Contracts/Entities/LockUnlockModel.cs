namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class LockUnlockModel
    {
        public string all_items { get; set; }
        public string grid_locn_id { get; set; }
        public string lock_code { get; set; }
        public string sku_id { get; set; }
    }
}