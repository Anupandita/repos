using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sfc.Core.OnPrem.Result;
using FunctionalTestProject.SQLQueries;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using System.Collections.Generic;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class ReceivingFixture:BaseFixture
    {

        protected int TotalRecords = 0;
        protected DataTable ReceivingSearchQueryDt=new DataTable();
        protected DataTable ReceivingSearchResultDt = new DataTable();
        protected DataTable ReceivingDetailsQueryDt = new DataTable();
        protected DataTable ReceivingDetailsResultDt = new DataTable();
        protected DataTable ReceivingDetailsDrilldownQueryDt = new DataTable();
        protected DataTable ReceivingDetailsDrilldownResultDt = new DataTable();
        protected DataTable ReceivingQVDetailsQueryDt = new DataTable();
        protected DataTable ReceivingQVDetailsResultDt = new DataTable();
        protected DataTable CorbaOutputDt = new DataTable();
        AnswerTextDto answerTextDto;
        UpdateAsnDto updateAsnDto;
        public void PickAnIReceivingTestDataFromDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(ReceiptInquiryQueries.FetchPoNbrSql, db);
                UIConstants.PoNumber = _command.ExecuteScalar().ToString();

                _command = new OracleCommand(ReceiptInquiryQueries.GetReceivingSearchByStatusCount(), db);
                UIConstants.ReceivingStatusCount = _command.ExecuteScalar().ToString();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchQvShipmentNbrSql, db);
                UIConstants.QvShipmentNbr = _command.ExecuteScalar().ToString();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchAsnNbrSql, db);
                DataTable tempDt = new DataTable();
                tempDt.Load(_command.ExecuteReader());
                UIConstants.ShipmentNbr = tempDt.Rows[0][0].ToString();
                UIConstants.ShipmentNbrCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchVendorName, db);              
                tempDt.Load(_command.ExecuteReader());
                UIConstants.VendorName = tempDt.Rows[0][0].ToString();
                UIConstants.VendorNameCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchVerfDateSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.VerifiedFrom = tempDt.Rows[0][0].ToString();
                UIConstants.VerifiedTo = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchUnAnsQvShipmentNbrSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.UnAnsQvShipmentNbr = tempDt.Rows[0][0].ToString();
                UIConstants.QuesAnswerId= tempDt.Rows[0][0].ToString();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchMainPageGridDtSql(), db);
                ReceivingSearchQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ReceiptInquiryQueries.FetchDetailsGridDtSql(), db);
                ReceivingDetailsQueryDt.Load(_command.ExecuteReader());
                UIConstants.ItemNumber = ReceivingDetailsQueryDt.Rows[0][1].ToString();


                _command = new OracleCommand(ReceiptInquiryQueries.FetchDetailsDrilldownGridDataSql(), db);
                ReceivingDetailsDrilldownQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ReceiptInquiryQueries.FetchQvGridDataSql(), db);
                ReceivingQVDetailsQueryDt.Load(_command.ExecuteReader());


                _command = new OracleCommand(ReceiptInquiryQueries.GetReceivingSearchByVerifiedDateCount(), db);
                UIConstants.VerifiedDateRangeCount = _command.ExecuteScalar().ToString();


            }
        }
        public void CreateUrlAndInputParamForApiUsing(string criteria)
        {
            switch (criteria)
            {
                case "PoNumber":
                    UIConstants.ReceivingSearchUrl = UIConstants.ReceivingSearchUrl + "poNumber=" + UIConstants.PoNumber;
                    return;
                case "ShipmentNumber":
                    UIConstants.ReceivingSearchUrl = UIConstants.ReceivingSearchUrl + "shipmentNumber=" + UIConstants.ShipmentNbr;
                    return;
                case "VendorName":
                    UIConstants.ReceivingSearchUrl = UIConstants.ReceivingSearchUrl+"vendorName="+UIConstants.VendorName;
                    return;
                case "StatusRange":
                    UIConstants.ReceivingSearchUrl = UIConstants.ReceivingSearchUrl+ "statusFrom="+UIConstants.FromStatus+"&statusTo="+UIConstants.ToStatus;
                    return;
                case "VerifiedDateRange":
                    UIConstants.ReceivingSearchUrl = UIConstants.ReceivingSearchUrl+"verifiedFrom=" + UIConstants.VerifiedFrom+"&verifiedTo=" + UIConstants.VerifiedTo;
                    break;              
                case "Details":
                    UIConstants.ReceivingDetailsUrl = UIConstants.ReceivingSearchUrl + UIConstants.ShipmentNbr;
                    return;
                case "DetailsDrilldown":
                    UIConstants.ReceivingDetailsDrilldownUrl = UIConstants.ReceivingSearchUrl + UIConstants.ShipmentNbr+ UIConstants.ReceivingDetailsDrilldownUrl+UIConstants.ItemNumber;
                    return;
                case "QvDetails":
                    UIConstants.ReceivingQvDetailsUrl = UIConstants.ReceivingSearchUrl + UIConstants.QvShipmentNbr+ UIConstants.ReceivingQvDetailsUrl; 
                    return;
                case "UpdateQvDetails":
                    UIConstants.ReceivingQvDetailsUrl = UIConstants.ReceivingSearchUrl + UIConstants.UnAnsQvShipmentNbr + UIConstants.ReceivingQvDetailsUrl;
                    return;
            }
        }

        public void CallReceivingSearchApiWithInputs(string url)
        {
            var response=CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<SearchResultDto>>(response.Content).Payload;
            ReceivingSearchResultDt = ToDataTable(payload.ReceivingDetailsDtos);
            TotalRecords = payload.TotalRecords;
        }
        public void VerifyReceivingSearchOutputAgainstDbOutput() 
        {
            VerifyApiOutputAgainstDbOutput(ReceivingSearchQueryDt, ReceivingSearchResultDt);                      
        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            VerifyOutputTotalReordsAgainstDbCount(count, TotalRecords.ToString());
        }

         public void CallReceivingDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<AsnDetailDto>>>(response.Content).Payload;
            ReceivingDetailsQueryDt = ToDataTable(payload);

        }
        public void CallReceivingDetailsDrilldownApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<AsnLotTrackingDto>>>(response.Content).Payload;
            ReceivingDetailsDrilldownQueryDt = ToDataTable(payload);

        }     
        public void VerifyOutputForReceivingDetailsInApiOutput() 
        {
            VerifyApiOutputAgainstDbOutput(ReceivingDetailsQueryDt, ReceivingDetailsResultDt);
        }
        public void VerifyOutputForReceivingDetailsDrilldownInApiOutput()
        {
            VerifyApiOutputAgainstDbOutput(ReceivingDetailsDrilldownQueryDt, ReceivingDetailsDrilldownResultDt);
               
        }
        public void CallReceivingUpdateQvDetailsApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(answerTextDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
        public void CallReceivingQvDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<QvDetailsDto>>>(response.Content).Payload;
            ReceivingQVDetailsQueryDt = ToDataTable(payload);
        }
        public void VerifyOutputForQvDetailsInApiOutput()
        {
            VerifyApiOutputAgainstDbOutput(ReceivingQVDetailsQueryDt, ReceivingQVDetailsResultDt);
        }
        public void CreateAnswerInput()
        {
            answerTextDto = new AnswerTextDto()
            {
                AnswerText = UIConstants.RefValue,
                QuestionAnswerId = UIConstants.QuesAnswerId,
            };
        }
        public void VerifyQVDetailsUpdatedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(ReceiptInquiryQueries.VerifyAnswerUpdatedSql(), db);
                Assert.IsNotNull(_command.ExecuteScalar().ToString());               
            }
        }

        public void CreateInputDtoFor(string btnName) 
        {
            switch (btnName)
            {
                case "Verify Receipt": 
                    {
                        updateAsnDto = new UpdateAsnDto()
                        {
                            Action = "verify-receipt",
                            WorkStationId = UIConstants.WorkStation,
                                   
                            
                        }; 
                            break;
                    }
                case "Multi Item": 
                    
                    {
                        updateAsnDto = new UpdateAsnDto()
                        {
                            Action = "multi-item",
                            WorkStationId = UIConstants.WorkStation,
                            

                        }; 
                        break;}
                case "NLL": 
                    {
                        updateAsnDto = new UpdateAsnDto()
                        {
                            Action = "nll",
                            WorkStationId = UIConstants.WorkStation,
                            

                        }; 
                        break;
                    }
            }
        }
        public void CallReceivingCorbaApi(string url) 
        {
            var request = CallPutApi();
            request.AddJsonBody(answerTextDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
        public void VerifyCorbaResultFromDbFor(string btnName) 
        {
            string sql="";
            switch (btnName)
            {
                case "Verify Receipt": 
                    { 
                        sql = "select * from pb2_corba_dtl where id in (select max(ph.id) from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where func_name like '%" + FuncName.VerifyShipment + "' and parm_name='shipment' and parm_value='" + UIConstants.ShipmentNbr + "' and crt_date like sysdate) and parm_name='return'";
                        break;
                    }
                case "Multi Item": 
                    { 
                        sql = "select * from pb2_corba_dtl where id in (select id from pb2_corba_dtl where id in (select max(ph.id) from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where func_name like '%printShipmentPB' and parm_name='shipment' and parm_value='12376487' and crt_date like sysdate-1) and parm_name='mode' and parm_value='P') and parm_name ='return'"; 
                        break;
                    }
                case "NLL": 
                    {
                        sql = "select * from pb2_corba_dtl where id in (select id from pb2_corba_dtl where id in (select max(ph.id) from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where func_name like '%printShipmentPB' and parm_name='shipment' and parm_value='12376487' and crt_date like sysdate-1) and parm_name='mode' and parm_value='R') and parm_name ='return'"; 
                        break;
                    }
            }
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(sql, db);
                Assert.AreEqual("PkValid", _command.ExecuteScalar());
            }
        }

    }
}
