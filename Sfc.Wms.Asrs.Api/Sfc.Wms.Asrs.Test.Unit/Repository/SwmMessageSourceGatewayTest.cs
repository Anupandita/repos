using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Repository
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To perform Create Read Update Delete operations on SwmMessageSource table",
        SoThat = "Create Read Update Delete operations are done on SwmMessageSource table"
    )]
    public class SwmMessageSourceGatewayTest : SwmMessageSourceGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_SwmMessageSource_Details_Gateway_Invocation_Returned_With_No_Records()
        {
            this.Given(el => el.QueryParametersForWhichRecordDoesNotExists())
                .When(el => el.GetSwmMessageSourceDetailsByKeyGatewayInvoked())
                .Then(el => el.TheInvokedGetAllSwmMessageSourceShouldNotReturnAnyRecords())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_SwmMessageSource_Details_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestKey())
                .When(el => el.GetSwmMessageSourceDetailsByKeyGatewayInvoked())
                .Then(el => el.TheInvokedGetAllSwmMessageSourceShouldReturnedWithAllRecords())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_SwmMessageSource_Gateway_Invocation_Returned_Conflict_Status_As_Response()
        {
            this.Given(el => el.InputRequestDataForWhichRecordAlreadyExists())
                .When(el => el.InsertSwmMessageSourceGatewayInvoked())
                .Then(el => el.TheInvokedInsertSwmMessageSourceShouldReturnedWithConflictResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_SwmMessageSource_Gateway_Invocation_Returned_Created_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestDataForInsert())
                .When(el => el.InsertSwmMessageSourceGatewayInvoked())
                .Then(el => el.TheReturnedResponseStatusIsCreated())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_SwmMessageSource_Gateway_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputRequestDataForWhichRecordDoesNotExists())
                .When(el => el.UpdateSwmMessageSourceGatewayInvoked())
                .Then(el => el.TheUpdateOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_SwmMessageSource_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestDataForUpdate())
                .When(el => el.UpdateSwmMessageSourceGatewayInvoked())
                .Then(el => el.TheUpdateSwmMessageSourceOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_SwmMessageSource_Gateway_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputRequestKeyForWhichRecordDoesNotExists())
                .When(el => el.DeleteSwmMessageSourceByKeyGatewayInvoked())
                .Then(el => el.TheDeleteOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_SwmMessageSource_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestKeyForDelete())
                .When(el => el.DeleteSwmMessageSourceByKeyGatewayInvoked())
                .Then(el => el.TheDeleteSwmMessageSourceOperationReturnedOkResponse())
                .BDDfy();
        }
    }
}