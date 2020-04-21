using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass, TestCategory(TestCategories.Unit)]
    public class ReceivingGatewayTest:ReceivingGatewayFixture
    {
        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Receiving_Search_Operation_Returned_Ok_As_Response_Status()
        {
            InputForReceivingSearch();
            ReceivingSearchOperationInvoked();
            TheLpnSearchOperationReturnedOkAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Receiving_Search_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullSearchParameters();
            ReceivingSearchOperationInvoked();
            TheReceivingSearchOperationReturnedBadRequestAsResponseStatus();
        }

       [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_AsnLotTracking_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToFetchAsnLotTrackingDetails();
            FetchAsnLotTrackingDetailsOperationInvoked();
            FetchAsnLotTrackingDetailsReturnedOkAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_AsnLotTracking_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullToFetchAsnLotTrackingDetails();
            FetchAsnLotTrackingDetailsOperationInvoked();
            FetchAsnLotTrackingDetailsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_AsnLotTracking_Details_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToFetchAsnLotTrackingDetailsForWhichNoDetailsExists();
            FetchAsnLotTrackingDetailsOperationInvoked();
            FetchAsnLotTrackingDetailsReturnedNotFoundAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_Answer_Text_Operation_Returned_Ok_As_Response_Status()
        {
            InputToUpdateAnswerText();
            UpdateAnswerTextOperationInvoked();
            UpdateAnswerTextReturnedOkAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_Answer_Text_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullToUpdateAnswerText();
            UpdateAnswerTextOperationInvoked();
            UpdateAnswerTextReturnedBadRequestAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_Answer_Text_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToUpdateAnswerTextForWhichNoDetailsExists();
            UpdateAnswerTextOperationInvoked();
            UpdateAnswerTextReturnedNotFoundAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToFetchAsnDetails();
            FetchAsnDetailsOperationInvoked();
            FetchAsnDetailsReturnedOkAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullToFetchAsnDetails();
            FetchAsnDetailsOperationInvoked();
            FetchAsnDetailsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Asn_Details_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToFetchAsnDetailsForWhichNoDetailsExists();
            FetchAsnDetailsOperationInvoked();
            FetchAsnDetailsReturnedNotFoundAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Qv_Details_Operation_Returned_Ok_As_Response_Status()
        {
            InputToFetchQvDetails();
            FetchQvDetailsOperationInvoked();
            FetchQvDetailsReturnedOkAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Qv_Details_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullToFetchQvDetails();
            FetchQvDetailsOperationInvoked();
            FetchQvDetailsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Get_Qv_Details_Operation_Returned_NotFound_As_Response_Status()
        {
            InputToFetchQvDetailsForWhichNoDetailsExists();
            FetchQvDetailsOperationInvoked();
            FetchQvDetailsReturnedNotFoundAsResponseStatus();
        }
    }
}
