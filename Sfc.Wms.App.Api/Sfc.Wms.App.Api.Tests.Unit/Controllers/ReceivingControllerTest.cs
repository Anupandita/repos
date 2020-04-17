using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass, TestCategory(TestCategories.Unit)]
    public class ReceivingControllerTest:ReceivingControllerFixture
    {
        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Receipt_Inquiry_Search_Operation_Returned_Ok_As_Response_Status()
        {
            InputForReceiptInquiry();
            SearchOperationInvoked();
            TheSearchOperationReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Receipt_Inquiry_Search_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputForReceiptInquiry();
            SearchOperationInvoked();
            TheSearchOperationReturnedBadRequestResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToFetchAsnDetails();
            FetchAsnDetailsOperationInvoked();
            TheFetchAsnDetailsOperationReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputToFetchAsnDetails();
            FetchAsnDetailsOperationInvoked();
            TheFetchAsnDetailsOperationReturnedBadRequestResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Qv_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToGetQvDetails();
            GetQvDetailsOperationInvoked();
            TheGetQvDetailsOperationReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Qv_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputToGetQvDetails();
            GetQvDetailsOperationInvoked();
            TheGetQvDetailsOperationReturnedBadRequestResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Lot_Track_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToGetAsnLotTrackDetails();
            GetAsnLotTrackDetailsOperationInvoked();
            TheGetAsnLotTrackDetailsOperationReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Lot_Track_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputToGetAsnLotTrackDetails();
            GetAsnLotTrackDetailsOperationInvoked();
            TheGetAsnLotTrackDetailsOperationReturnedBadRequestResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Lot_Track_Details_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToGetAsnLotTrackDetailsForWhichNoRecordExists();
            GetAsnLotTrackDetailsOperationInvoked();
            TheGetAsnLotTrackDetailsOperationReturnedNotFoundAsResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_AnswetText_Operation_Returned_Ok_As_Response_Status()
        {
            InputToUpdateAnswerText();
            UpdateAnswerTextOperationInvoked();
            TheUpdateAnswerTextOperationReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_Answer_Text_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputToUpdateAnswerText();
            UpdateAnswerTextOperationInvoked();
            TheUpdateAnswerTextOperationReturnedBadRequestResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_Answer_Text_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToUpdateAnswerTextForWhichNoRecordExists();
            UpdateAnswerTextOperationInvoked();
            TheUpdateAnswerTextOperationReturnedNotFoundAsResponse();
        }
    }
}
