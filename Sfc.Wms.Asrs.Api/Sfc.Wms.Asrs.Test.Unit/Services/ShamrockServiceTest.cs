using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Services
{
    [TestClass]
    [Story(
       AsA = "End User",
       IWant = "To perform Create Read Update Delete operations on SwmFromMhe table",
       SoThat = "Create Read Update Delete operations are done on SwmFromMhe table"
       )]
    public class ShamrockServiceTest : ShamrockServiceFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Getall_Service_Is_Invoked_With_No_Query_Parameters_Ok_Is_Returned()
        {
            this.Given(CustomMessages.NoFilterInRequest)
                .When(el => el.GetAllOperationIsInvoked())
                .Then(el => el.TheGetAllReturnedOkResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Details_By_Key_Is_Invoked_With_Invalid_Query_Parameters_NotFound_Is_Returned()
        {
            this.Given(el => el.QueryParametersForWhichRecordDoesNotExists())
                .When(el => el.GetDetailsByKeyIsInvoked())
                .Then(el => el.TheGetDetailsByKeyReturnedNotFound()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Details_By_Key_Is_Invoked_With_Valid_Query_Parameters_Ok_Is_Returned()
        {
            this.Given(el => el.QueryParametersForWhichRecordExists())
                .When(el => el.GetDetailsByKeyIsInvoked())
                .Then(el => el.TheGetDetailsByKeyReturnedOkStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Operation_Is_Invoked_With_Duplicate_Data_Conflict_Is_Returned()
        {
            this.Given(el => el.InputRequestDataWhichConfilctsWithExistingRecords())
                .When(el => el.AddOperationIsInvoked())
                .Then(el => el.TheAddOperationReturnedConflictStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Operation_Is_Invoked_With_Valid_Data_Created_Response_Is_Returned()
        {
            this.Given(el => el.ValidRecordIsPassedToAddService())
                .When(el => el.AddOperationIsInvoked())
                .Then(el => el.TheAddOperationReturnedCreatedStatusAsResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            this.Given(el => el.UpdateForWhichNoRecordExists())
                .When(el => el.UpdateOperationIsInvoked())
                .Then(el => el.TheUpdateOperationReturnedNotFoundStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Operation_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            this.Given(el => el.ValidRecordIsPassedToUpdateService())
                .When(el => el.UpdateOperationIsInvoked())
                .Then(el => el.TheUpdateOperationReturnedOkStatusAsResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            this.Given(el => el.DeleteForWhichNoRecordExists())
                .When(el => el.DeleteOperationIsInvoked())
                .Then(el => el.TheDeleteServiceOperationReturnedNotFoundStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Service_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            this.Given(el => el.ValidKeyIsPassedToDeleteService())
                .When(el => el.DeleteOperationIsInvoked())
                .Then(el => el.TheDeleteServiceOperationReturnedOkStatusAsResponse()).BDDfy();
        }
    }
}
