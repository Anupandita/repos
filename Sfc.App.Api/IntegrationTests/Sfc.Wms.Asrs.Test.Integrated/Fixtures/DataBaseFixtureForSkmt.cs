using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Newtonsoft.Json;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForSkmt : CommonFunction
    {
        protected readonly ItemMasterView ItemMaster = new ItemMasterView();
        protected ItemMasterView Normal = new ItemMasterView();
        protected ItemMasterView ParentSku = new ItemMasterView();
        protected ItemMasterView ChildSku = new ItemMasterView();
        protected ItemMasterView Childskuassertion = new ItemMasterView();
        protected WmsToEmsDto WmsToEmsSkmt = new WmsToEmsDto();
        protected SwmToMheDto SwmToMheSkmt = new SwmToMheDto();
        protected SkmtDto Skmt = new SkmtDto();
        public string Uom;

        public void GetDataBeforeTriggerSkmt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Normal = TriggerOnItemMaster(db, null);
                var UOM = GetUnitOfMeasureFromItemMaster(db, Normal.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
            }
        }

        public void GetDataBeforeTriggerSkmtParent()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                ParentSku = TriggerOnItemMaster(db, Constants.Child);
                var UOM = GetUnitOfMeasureFromItemMaster(db, ParentSku.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);

            }
        }
        public void GetDataBeforeTriggerSkmtChildSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                ParentSku = TriggerOnItemMaster(db, Constants.Child);
                ChildSku = TriggerOnItemMaster(db, Constants.Parent);
                Childskuassertion = ChildSkufunction(db, ParentSku.SkuId);
                var UOM = GetUnitOfMeasureFromItemMaster(db, ParentSku.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
            }
        }

        public ItemMasterView TriggerOnItemMaster(OracleConnection db, string skuCondition)
        {
            var sqlStatement = $"select * from Item_master";
            if (skuCondition != null)
            {
                sqlStatement = sqlStatement + $" where SPL_INSTR_CODE_5='{skuCondition}'";
            }
            Command = new OracleCommand(sqlStatement, db);
            var itemMasterReader = Command.ExecuteReader();
            if (itemMasterReader.Read())
            {
                ItemMaster.SkuId = itemMasterReader[ItemMasterViews.SkuId].ToString();
                ItemMaster.Div = itemMasterReader[ItemMasterViews.Div].ToString();
                ItemMaster.Skudesc = itemMasterReader[ItemMasterViews.SkuDesc].ToString();
                ItemMaster.StdCaseQty = itemMasterReader[ItemMasterViews.StdCaseQty].ToString();
                ItemMaster.Tempzone = itemMasterReader[ItemMasterViews.Tempzone].ToString();
                ItemMaster.Unitwieght = itemMasterReader[ItemMasterViews.Unitwieght].ToString();
                ItemMaster.Unitvolume = itemMasterReader[ItemMasterViews.Unitvolume].ToString();
                ItemMaster.Prodlifeinday = itemMasterReader[ItemMasterViews.Prodlifeinday].ToString();
                ItemMaster.NestVolume = itemMasterReader[ItemMasterViews.NestVolume].ToString();
                ItemMaster.Skubrcd = itemMasterReader[ItemMasterViews.Skubrcd].ToString();
                ItemMaster.Colordescription = itemMasterReader[ItemMasterViews.Colordesc].ToString();
            }
            return ItemMaster;
        }

        public ItemMasterView ChildSkufunction(OracleConnection db, string colordesc)
        {
            var query = $"select * from Item_master WHERE COLOR_DESC= :colordesc";
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("colordesc", colordesc));
            var colordescReader = Command.ExecuteReader();
            if (colordescReader.Read())
            {
                ItemMaster.SkuId = colordescReader[ItemMasterViews.SkuId].ToString();
                ItemMaster.Colordescription = colordescReader[ItemMasterViews.Colordesc].ToString();

            }
            return ItemMaster;
        }

        protected void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheSkmt = SwmToMhe(db,null, TransactionCode.Skmt, ItemMaster.SkuId);                  
                Skmt = JsonConvert.DeserializeObject<SkmtDto>(SwmToMheSkmt.MessageJson);
                WmsToEmsSkmt = WmsToEmsData(db, SwmToMheSkmt.SourceMessageKey, TransactionCode.Skmt);
            }
        }
    }
}
