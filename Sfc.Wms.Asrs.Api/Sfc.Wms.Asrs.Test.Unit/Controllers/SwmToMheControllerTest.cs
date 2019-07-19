using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;
using Sfc.Wms.Asrs.Test.Unit.Constants;
namespace Sfc.Wms.Asrs.Test.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To perform Create Read Update Delete operations on SwmToMhe table",
        SoThat = "Create Read Update Delete operations are done on SwmToMhe table"
        )]
    public class SwmToMheControllerTest : SwmToMheFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAll_Is_Called_With_No_Query_Ok_Is_Returned()
        {
            this.Given(CustomMessages.NoFilterInRequest)
                .When(el => el.GetAllIsInvoked())
                .Then(el => el.TheReturnedResponseStatusOfGetAllIsOk()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Is_Called_With_Invalid_Query_Parameters_NotFound_Is_Returned()
        {
            IsValidData = false;
            this.Given(el => el.QueryParametersForWhichRecordDoesNotExists())
                .When(el => el.GetOperationIsInvoked())
                .Then(el => el.TheGetOperationReturnedNotFoundStatusAsResponse()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Is_Called_With_Valid_Query_Parameters_Ok_Is_Returned()
        {
            IsValidData = true;
            this.Given(el => el.ValidInputDataInRequest())
                .When(el => el.GetOperationIsInvoked())
                .Then(el => el.TheGetByKeyOperationReturnedOkResponseStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Operation_Is_Invoked_With_InValid_Data_NotFound_Is_Returned()
        {
            IsValidData = false;
            this.Given(el => QueryParametersForWhichRecordDoesNotExists())
                .When(el => UpdateOperationIsInvoked())
                .Then(el => TheInvokeReturnedNotFoundResponse()).BDDfy();
        }
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Operation_Is_Invoked_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            this.Given(el => ValidInputDataInRequest())
                .When(el => el.UpdateOperationIsInvoked())
                .Then(el => TheUpdateOperationReturnedOkResponseStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Operation_Is_Invoked_With_InValid_Query_Parameters_NotFound_Is_Returned()
        {
            IsValidData = false;
            this.Given(el => QueryParametersForWhichRecordDoesNotExists())
                .When(el => DeleteOperationIsInvoked())
                .Then(el => TheInvokeReturnedNotFoundResponse()).BDDfy();
        }
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Operation_Is_Invoked_With_Valid_Query_Parameters_Ok_Is_Returned()
        {
            IsValidData = true;
            this.Given(el => ValidInputDataInRequest())
                .When(el => el.DeleteOperationIsInvoked())
                .Then(el => TheDeleteOperationReturnedOkResponseStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Operation_Called_With_Duplicate_Data_Conflict_Is_Returned()
        {
            IsValidData = false;
            this.Given(el => QueryParametersForWhichRecordDoesNotExists())
                .When(el => InsertOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsConflict()).BDDfy();
        }
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Operation_Called_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            this.Given(el => ValidInputDataInRequest())
                .When(el => el.InsertOperationIsInvoked())
                .Then(el => TheInsertOperationReturnedOkResponseStatus()).BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_By_Status_Is_Called_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            this.Given(el => ValidInputDataInRequest())
                .When(el => GetByStatusOperationIsInvoked())
                .Then(el => TheGetByStatusOperationReturnedOkResponseStatus()).BDDfy();
        }

    }
}
