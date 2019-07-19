using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "108268 Create a package to perform crud operation on the EMSTOWMS table",
        IWant = "To perform CRUD operations on EmsToWms table",
        SoThat = "Create Read Update Delete operations are done on EmsToWms table"
    )]
    public class EmsToWmsControllerTest : EmsToWmsFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAll_Is_Called_With_No_Query_Ok_Is_Returned()
        {
            this.Given("No filter in request")
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
        public void Get_Is_Called_With_Empty_Query_Parameters_BadRequest_Is_Returned()
        {
            IsValidData = false;
            this.Given(el => el.EmptyQueryParameters())
                .When(el => el.GetOperationIsCalled())
                .Then(el => el.TheGetOperationReturnedBadStatusAsResponse()).BDDfy();
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
        public void Update_Operation_With_Input_Data_For_Which_No_Record_Exists_Returned_NotFound_Status()
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