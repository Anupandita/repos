using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Dematic.Test.Unit.TestData;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Dematic.Test.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "108597 Create a package to perform crud operation on the WMSTOEMS table",
        IWant = "To perfoem CRUD operations on WmsToW table",
        SoThat = "CRUD operations are done on WmsToEms table"
        )]
    public class WmsToWmsControllerTest : WmsToEmsFixture
    {

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_GetAll_Is_Called_With_No_Query_Ok_Is_Returned()
        {
            ActionMethodToExecute = ActionNames.GetAll;
            this.Given("No filter in request")
                .When(el => GetAllIsCalled())
               .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Get_Is_Called_With_Invalid_Query_Paramerters_NotFound_Is_Returned()
        {
            IsValidData = false;
            ActionMethodToExecute = ActionNames.Get;
            this.Given(el => InvalidDataInRequest())
                .When(el => GetOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsNotFound()).BDDfy();
        }


        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Get_Is_Called_With_Valid_Query_Paramerters_Ok_Is_Returned()
        {
            IsValidData = true;
            ActionMethodToExecute = ActionNames.Get;

            this.Given(el => ValidInputDataInRequest())
                .When(el => GetOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Update_Is_Called_With_InValid_Data_NotFound_Is_Returned()
        {
            IsValidData = false;
            ActionMethodToExecute = ActionNames.Update;
            this.Given(el => InvalidDataInRequest())
                .When(el => UpdateOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsNotFound()).BDDfy();
        }
        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Update_Is_Called_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            ActionMethodToExecute = ActionNames.Update;
            this.Given(el => ValidInputDataInRequest())
                .When(el => UpdateOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Delete_Is_Called_With_InValid_Query_Paramerters_NotFound_Is_Returned()
        {
            IsValidData = false;
            ActionMethodToExecute = ActionNames.Delete;
            this.Given(el => InvalidDataInRequest())
                .When(el => DeleteOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsNotFound()).BDDfy();
        }
        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Delete_Is_Called_With_Valid_Query_Paramerters_Ok_Is_Returned()
        {
            IsValidData = true;
            ActionMethodToExecute = ActionNames.Delete;
            this.Given(el => ValidInputDataInRequest())
                .When(el => DeleteOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Insert_Is_Called_With_Duplicate_Data_Conflict_Is_Returned()
        {
            IsValidData = false;
            ActionMethodToExecute = ActionNames.Insert;
            this.Given(el => InvalidDataInRequest())
                .When(el => InsertOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsConflict()).BDDfy();
        }
        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Insert_Is_Called_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            ActionMethodToExecute = ActionNames.Insert;
            this.Given(el => ValidInputDataInRequest())
                .When(el => InsertOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }

        [TestMethod()]
        [TestCategory("UNIT")]
        public void When_Get_By_Status_Is_Called_With_Valid_Data_Ok_Is_Returned()
        {
            IsValidData = true;
            ActionMethodToExecute = ActionNames.GetByStatus;
            this.Given(el => ValidInputDataInRequest())
                .When(el => GetByStatusOperationIsInvoked())
                .Then(el => TheReturnedResponseStatusIsOk()).BDDfy();
        }
       
    }
}
