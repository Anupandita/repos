using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        protected ItemMasterView ItemMaster = new ItemMasterView();
        protected ItemMasterView Null = new ItemMasterView();
        protected ItemMasterView parentSku = new ItemMasterView();
        protected ItemMasterView childSku = new ItemMasterView();
        protected ItemMasterView Childskuassertion = new ItemMasterView();
        protected WmsToEmsDto wmsToEmsSkmt = new WmsToEmsDto();
        protected SwmToMheDto swmToMheSkmt = new SwmToMheDto();
        protected SkmtDto skmt = new SkmtDto();


        public void GetDataBeforeTriggerSkmt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Null = TriggerOnItemMaster(db, null);
            }
        }

        public void GetDataBeforeTriggerSkmtParent()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                parentSku = TriggerOnItemMaster(db, "C");

            }
        }
        public void GetDataBeforeTriggerSkmtChildSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                parentSku = TriggerOnItemMaster(db, "C");
                childSku = TriggerOnItemMaster(db, "P");
                Childskuassertion = ChildSkufunction(db, parentSku.SkuId);
            }


        }

        public ItemMasterView TriggerOnItemMaster(OracleConnection db, string skuCondition)
        {
            var sqlStatement = $"select * from Item_master";
            if (skuCondition != null)
            {
                sqlStatement = sqlStatement + $" where SPL_INSTR_CODE_5='{skuCondition}'";
            }
            command = new OracleCommand(sqlStatement, db);
            var ItemMasterReader = command.ExecuteReader();
            if (ItemMasterReader.Read())
            {
                ItemMaster.SkuId = ItemMasterReader[ItemMasterViews.SkuId].ToString();
                ItemMaster.div = ItemMasterReader[ItemMasterViews.div].ToString();
                ItemMaster.Skudesc = ItemMasterReader[ItemMasterViews.skuDesc].ToString();
                ItemMaster.StdCaseQty = ItemMasterReader[ItemMasterViews.StdCaseQty].ToString();
                ItemMaster.tempzone = ItemMasterReader[ItemMasterViews.tempzone].ToString();
                ItemMaster.unitwieght = ItemMasterReader[ItemMasterViews.unitwieght].ToString();
                ItemMaster.unitvolume = ItemMasterReader[ItemMasterViews.unitvolume].ToString();
                ItemMaster.prodlifeinday = ItemMasterReader[ItemMasterViews.prodlifeinday].ToString();
                ItemMaster.NestVolume = ItemMasterReader[ItemMasterViews.NestVolume].ToString();
                ItemMaster.skubrcd = ItemMasterReader[ItemMasterViews.skubrcd].ToString();
                ItemMaster.colordescription = ItemMasterReader[ItemMasterViews.colordesc].ToString();
            }


            return ItemMaster;

        }


        public ItemMasterView ChildSkufunction(OracleConnection db, string colordesc)
        {
            var Query = $"select * from Item_master WHERE COLOR_DESC='{colordesc}'";

            command = new OracleCommand(Query, db);
            var ColordescReader = command.ExecuteReader();
            if (ColordescReader.Read())
            {
                ItemMaster.SkuId = ColordescReader[ItemMasterViews.SkuId].ToString();
                ItemMaster.colordescription = ColordescReader[ItemMasterViews.colordesc].ToString();

            }
            return ItemMaster;

        }

        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                command = new OracleCommand();
                swmToMheSkmt = SwmToMhe(db, ItemMaster.SkuId, TransactionCode.Skmt);
                skmt = JsonConvert.DeserializeObject<SkmtDto>(swmToMheSkmt.MessageJson);
                wmsToEmsSkmt = WmsToEmsData(db, swmToMheSkmt.SourceMessageKey, TransactionCode.Skmt);

            }
        }

        protected SwmToMheDto SwmToMhe(OracleConnection db, string skuId, string trx)
        {
            var swmtomhedata = new SwmToMheDto();
            var query = $"select * from SWM_TO_MHE where sku_id = '{skuId}'and source_msg_trans_code = '{trx}' order by SOURCE_MSG_KEY desc";
            command = new OracleCommand(query, db);
            var swmToMheReader = command.ExecuteReader();
            if (swmToMheReader.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgKey].ToString());
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgRsnCode].ToString());
                swmtomhedata.SourceMessageStatus = swmToMheReader[TestData.SwmToMhe.SourceMsgStatus].ToString();
                swmtomhedata.ContainerId = swmToMheReader[TestData.SwmToMhe.ContainerId].ToString();
                swmtomhedata.ContainerType = swmToMheReader[TestData.SwmToMhe.ContainerType].ToString();
                swmtomhedata.MessageJson = swmToMheReader[TestData.SwmToMhe.MsgJson].ToString();
                swmtomhedata.LocationId = swmToMheReader[TestData.SwmToMhe.LocnId].ToString();
                swmtomhedata.SourceMessageText = swmToMheReader[TestData.SwmToMhe.SourceMsgText].ToString();
            }
            return swmtomhedata;
        }

    }
}
