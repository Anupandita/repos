using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Repository
{
    [TestClass]
    [Story(
        AsA = "108268 Create a package to perform crud operation on the EMSTOWMS table",
        IWant = "To perform Create Read Update Delete operations on EmsToWms table",
        SoThat = "Create Read Update Delete operations are done on EmsToWms table"
    )]
    public class DematicGatewayTest : DematicGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Details_Gateway_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.QueryParamertesForWhichRecordDoesNotExists())
                .When(el => el.GetDetailsByKeyGatewayInvoked())
                .Then(el => el.TheReturnedResponseStatusIsNotfound())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Details_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestKey())
                .When(el => el.GetDetailsByKeyGatewayInvoked())
                .Then(el => el.TheReturnedResponseStatusIsOk())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_Gateway_Invocation_Returned_Conflict_Status_As_Response()
        {
            this.Given(el => el.InputRequestDataForWhichRecordAlreadyExists())
                .When(el => el.InsertGatewayInvoked())
                .Then(el => el.TheReturnedResponseStatusIsConflicted())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_Gateway_Invocation_Returned_Created_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestDataForInsert())
                .When(el => el.InsertGatewayInvoked())
                .Then(el => el.TheReturnedResponseStatusIsCreated())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Gateway_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputRequestDataForWhichRecordDoesNotExists())
                .When(el => el.UpdateGatewayInvoked())
                .Then(el => el.TheUpdateOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestDataForUpdate())
                .When(el => el.UpdateGatewayInvoked())
                .Then(el => el.TheUpdateOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Gateway_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputRequestKeyForWhichRecordDoesNotExists())
                .When(el => el.DeleteByKeyGatewayInvoked())
                .Then(el => el.TheDeleteOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Gateway_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidInputRequestKeyForDelete())
                .When(el => el.DeleteByKeyGatewayInvoked())
                .Then(el => el.TheDeleteOperationReturnedOkResponse())
                .BDDfy();
        }
    }
}