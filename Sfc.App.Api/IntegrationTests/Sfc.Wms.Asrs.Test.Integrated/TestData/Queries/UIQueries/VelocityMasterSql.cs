using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace FunctionalTestProject.SQLQueries
{
    public static class VelocityMasterSql
    {
        public const string FetchSKuSql = "select distinct sku_id from sku_vel_master ORDER BY dbms_random.value";
        public static string FetchSkuVelocityMasterDt()
        {
            return $"SELECT whse,sku_id,sku_desc,vel_13,vel_4,vel_1 from sku_vel_master svm WHERE svm.sku_id='{UIConstants.ItemNumber}'";
        }
    }
}
