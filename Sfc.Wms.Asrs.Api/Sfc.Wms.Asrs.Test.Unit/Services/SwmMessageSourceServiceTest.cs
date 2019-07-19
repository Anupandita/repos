using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Services
{
    [TestClass]
    [Story(
       AsA = "End User",
       IWant = "To perform Create Read Update Delete operations on SwmMessageSource table",
       SoThat = "Create Read Update Delete operations are done on SwmMessageSource table"
       )]
    public class SwmMessageSourceServiceTest : SwmMessageSourceServiceFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAll_SwmMessageSource_Service_Is_Invoked_With_No_Query_Parameters_Ok_Is_Returned()
        {
            this.Given(CustomMessages.NoFilterInRequest)
                .When(el => el.GetAllSwmMessageSourceOperationIsInvoked())
                .Then(el => el.TheGetAllSwmMessageSourceReturnedOkResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_SwmMessageSource_Details_By_Key_Is_Invoked_With_Invalid_Query_Parameters_NotFound_Is_Returned()
        {
            this.Given(el => el.QueryParametersForWhichRecordDoesNotExists())
                .When(el => el.GetSwmMessageSourceDetailsByKeyIsInvoked())
                .Then(el => el.TheGetSwmMessageSourceDetailsByKeyReturnedNotFound()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_SwmMessageSource_Details_By_Key_Is_Invoked_With_Valid_Query_Parameters_Ok_Is_Returned()
        {
            this.Given(el => el.QueryParametersForWhichRecordExists())
                .When(el => el.GetSwmMessageSourceDetailsByKeyIsInvoked())
                .Then(el => el.TheGetSwmMessageSourceDetailsByKeyReturnedOkStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_SwmMessageSource_Operation_Is_Invoked_With_Duplicate_Data_Conflict_Is_Returned()
        {
            this.Given(el => el.InputRequestDataWhichConfilctsWithExistingRecords())
                .When(el => el.InsertSwmMessageSourceOperationIsInvoked())
                .Then(el => el.TheInsertSwmMessageSourceOperationReturnedConflictStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_SwmMessageSource_Operation_Is_Invoked_With_Valid_Data_Created_Response_Is_Returned()
        {
            this.Given(el => el.ValidRecordIsPassedToAddService())
                .When(el => el.InsertSwmMessageSourceOperationIsInvoked())
                .Then(el => el.TheInsertSwmMessageSourceOperationReturnedCreatedStatusAsResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_SwmMessageSource_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            this.Given(el => el.UpdateForWhichNoRecordExists())
                .When(el => el.UpdateSwmMessageSourceOperationIsInvoked())
                .Then(el => el.TheUpdateSwmMessageSourceOperationReturnedNotFoundStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_SwmMessageSource_Operation_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            this.Given(el => el.ValidRecordIsPassedToUpdateService())
                .When(el => el.UpdateSwmMessageSourceOperationIsInvoked())
                .Then(el => el.TheUpdateSwmMessageSourceOperationReturnedOkStatusAsResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_SwmMessageSource_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            this.Given(el => el.DeleteForWhichNoRecordExists())
                .When(el => el.DeleteOperationIsInvoked())
                .Then(el => el.TheDeleteSwmMessageSourceServiceOperationReturnedNotFoundStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_SwmMessageSource_Service_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            this.Given(el => el.ValidKeyIsPassedToDeleteService())
                .When(el => el.DeleteOperationIsInvoked())
                .Then(el => el.TheDeleteSwmMessageSourceServiceOperationReturnedOkStatusAsResponse()).BDDfy();
        }
    }
}